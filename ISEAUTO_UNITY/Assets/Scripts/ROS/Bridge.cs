using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Int32 = DigitalTwin.ROS.std_msgs.Int32;

namespace DigitalTwin.ROS {

    /// <summary>
    /// Connector to rosbridge. There should only be one of these in the scene.
    /// </summary>
    public class Bridge : MonoBehaviour {
        public ConnectionType connectionType;
        
        public bool fragmentLongMessages;
        public int maxPacketSize = 1000;

        internal delegate void Callback(Message message);
        internal delegate void ServiceCallback(ServiceValues values);

        private List<string> advertisedTopics = new List<string>();
        private Dictionary<string, System.Type> subscribedTypes = new Dictionary<string, System.Type>();
        private Dictionary<string, Callback> subscribers = new Dictionary<string, Callback>();
        private Dictionary<string, ServiceCallback> serviceCallers = new Dictionary<string, ServiceCallback>();
        private Dictionary<string, System.Type> serviceTypes = new Dictionary<string, System.Type>();

        private Connection connection;

        public enum ConnectionType {
            UDP,
            TCP
        }

        
        private abstract class Connection {
            public bool Connected { get; set; }

            public abstract Task<byte[]> ReceiveAsync();
            public abstract byte[] Receive();
            public abstract void Close();
            public abstract void Send(byte[] data, int length);

            public abstract bool Available();
        }

        private class UDPConnection : Connection {
            private UdpClient udp;

            public UDPConnection(string ip, int port) {
                udp = new UdpClient(ip, port);
            }

            public override async Task<byte[]> ReceiveAsync() {
                var result = await udp.ReceiveAsync();
                return result.Buffer;
            }

            public override void Close() {
                udp.Close();
                Connected = false;
            }

            public override void Send(byte[] data, int length) {
                udp.Send(data, length);
            }

            public override byte[] Receive() {
                var endPoint = new IPEndPoint(IPAddress.Any, 0);
                return udp.Receive(ref endPoint);
            }

            public override bool Available() {
                return udp.Available > 0;
            }
        }

        private class TCPConnection : Connection {
            private TcpClient tcp;
            private NetworkStream stream;
            private byte[] buffer;

            public TCPConnection(string ip, int port) {
                tcp = new TcpClient(ip, port);
                stream = tcp.GetStream();
                buffer = new byte[tcp.ReceiveBufferSize];
            }

            public override bool Available() {
                throw new System.NotImplementedException();
            }

            public override void Close() {
                stream.Close();
                tcp.Close();
                Connected = false;
            }

            public override byte[] Receive() {
                throw new System.NotImplementedException();
            }

            public override async Task<byte[]> ReceiveAsync() {
                int length = await stream.ReadAsync(buffer, 0, buffer.Length);
                byte[] data = new byte[length];
                System.Buffer.BlockCopy(buffer, 0, data, 0, length);
                return data;
            }

            public override void Send(byte[] data, int length) {
                stream.Write(data, 0, length);
            }

            
        }

        [System.Serializable]
        private class RosBridgeMessage {
            public string op;
        }

        [System.Serializable]
        private class FragmentMessage : RosBridgeMessage {
            public string id;
            public string data;
            public int num;
            public int total;
        }

        [System.Serializable]
        private class TopicMessage : RosBridgeMessage {
            public string topic;
        }

        [System.Serializable]
        private class TopicTypeMessage : TopicMessage {
            public string type;
        }

        [System.Serializable]
        private class DataMessage<T> : TopicMessage where T : Message {
            public T msg;
        }

        [System.Serializable]
        private class RosBridgeService : RosBridgeMessage {
            public string service;
        }

        [System.Serializable]
        private class ServiceCall<A> : RosBridgeService where A : ServiceArgs {
            public A args;
        }

        [System.Serializable]
        private class ServiceResponse<V> : RosBridgeService where V : ServiceValues {
            public V values = default;
        }

        private Task connectionTask;

        private const string clientCountTopic = "client_count";

        /// <summary>
        /// Connect to a rosbridge udp server. This needs to be called before anything else.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port) {
            switch(connectionType) {
                case ConnectionType.UDP:
                    connection = new UDPConnection(ip, port);
                    break;
                case ConnectionType.TCP:
                    connection = new TCPConnection(ip, port);
                    break;
            }
            connectionTask = Task.Run(Reconnect);
        }

        private void Subscribe() {
            var subscribeMessage = new TopicTypeMessage {
                op = "subscribe",
                topic = clientCountTopic,
                type = new Int32().GetMessageType()
            };
            SendImmediate(JsonUtility.ToJson(subscribeMessage));
        }

        private void Reconnect() {
            while(connection != null && !connection.Connected) {
                Subscribe();
                Thread.Sleep(1000);
            }
        }

        private void Receive() {
            var data = connection.Receive();

            var json = Encoding.UTF8.GetString(data);

            var rosBridgeMessage = JsonUtility.FromJson<RosBridgeMessage>(json);
            var op = rosBridgeMessage.op;

            if(op == "service_response") {
                var serviceMessage = JsonUtility.FromJson<RosBridgeService>(json);
                var service = serviceMessage.service;

                var serviceResponce = JsonUtility.FromJson(json, serviceTypes[service]);
                var values = serviceTypes[service].GetField("values").GetValue(serviceResponce) as ServiceValues;
                serviceCallers[service](values);
                serviceCallers.Remove(service);
                serviceTypes.Remove(service);
            } else {
                var topicMessage = JsonUtility.FromJson<TopicMessage>(json);
                var topic = topicMessage.topic;

                if(topic == clientCountTopic) {
                    connection.Connected = true;
                    return;
                }

                var dataMessage = JsonUtility.FromJson(json, subscribedTypes[topic]);
                var message = subscribedTypes[topic].GetField("msg").GetValue(dataMessage) as Message;

                subscribers[topic](message);
            }
        }

        async internal void Advertise(string topic, string type) {
            await connectionTask;
            var advertiseMessage = new TopicTypeMessage {
                op = "advertise",
                topic = topic,
                type = type
            };
            Send(JsonUtility.ToJson(advertiseMessage));

            advertisedTopics.Add(topic);
        }

        internal void Publish<T>(string topic, T message) where T : Message {
            if(!advertisedTopics.Contains(topic)) {
                return;
            }
            var publishMessage = new DataMessage<T> {
                op = "publish",
                topic = topic,
                msg = message
            };
            var json = JsonUtility.ToJson(publishMessage);
            if(fragmentLongMessages && json.Length > maxPacketSize) {
                SendFragments(json);
            } else {
                Send(json);
            }
        }

        private int fragmentId = 0;

        private void SendFragments(string message) {
            int total = Mathf.CeilToInt((float)message.Length / (float)maxPacketSize);
            for(int n = 0; n < total; n++) {
                int length = maxPacketSize;
                int i = n * maxPacketSize;
                if(length > message.Length - i) {
                    length = message.Length - i;
                }
                var msgFragment = message.Substring(i, length);
                var fragment = new FragmentMessage {
                    op = "fragment",
                    data = msgFragment,
                    id = fragmentId.ToString(),
                    num = n,
                    total = total
                };
                Send(JsonUtility.ToJson(fragment));
            }
            fragmentId++;
        }

        async internal void Unadvertise(string topic, bool remove = true) {
            await connectionTask;
            var unadvertiseMessage = new TopicMessage {
                op = "unadvertise",
                topic = topic
            };
            Send(JsonUtility.ToJson(unadvertiseMessage));
            if(remove) {
                advertisedTopics.Remove(topic);
            }
        }

        async internal void Subscribe<T>(string topic, string type, Callback callback) where T : Message {
            await connectionTask;
            var subscribeMessage = new TopicTypeMessage {
                op = "subscribe",
                topic = topic,
                type = type
            };
            Send(JsonUtility.ToJson(subscribeMessage));
            if(!subscribedTypes.ContainsKey(topic))
                subscribedTypes.Add(topic, typeof(DataMessage<T>));
            if(!subscribers.ContainsKey(topic))
                subscribers.Add(topic, callback);
        }

        async internal void Unsubscribe(string topic, bool remove = true) {
            await connectionTask;
            var unsubscribeMessage = new TopicMessage {
                op = "unsubscribe",
                topic = topic
            };
            Send(JsonUtility.ToJson(unsubscribeMessage));
            if(remove) {
                subscribedTypes.Remove(topic);
                subscribers.Remove(topic);
            }
        }

        async internal void CallService<A, V>(string service, A args, ServiceCallback callback) where A : ServiceArgs where V : ServiceValues {
            await connectionTask;
            var callServiceMessage = new ServiceCall<A> {
                op = "call_service",
                service = service,
                args = args
            };
            if(!serviceTypes.ContainsKey(service))
                serviceTypes.Add(service, typeof(ServiceResponse<V>));
            if(!serviceCallers.ContainsKey(service))
                serviceCallers.Add(service, callback);
            Send(JsonUtility.ToJson(callServiceMessage));
        }

        private Queue<string> messagesToSend = new Queue<string>();

        private void SendImmediate(string json) {
            var bytes = Encoding.UTF8.GetBytes(json);
            connection.Send(bytes, bytes.Length);
        }

        private void FixedUpdate() {
            if(connection != null) {
                while(connection.Available()) {
                    Receive();
                }
                if(connection.Connected) {
                    while(messagesToSend.Count > 0) {
                        var json = messagesToSend.Dequeue();
                        SendImmediate(json);
                    }
                }
            }
        }

        private void Send(string json) {
            messagesToSend.Enqueue(json);
        }

        private void OnDisable() {
            if(connection != null) {
                if(connection.Connected) {
                    foreach(var topic in advertisedTopics) {
                        Unadvertise(topic, false);
                    }
                    foreach(var topic in subscribedTypes.Keys) {
                        Unsubscribe(topic, false);
                    }
                    Unsubscribe(clientCountTopic, false);
                }
                connection.Close();
                connection = null;
            }
        }
    }
}
