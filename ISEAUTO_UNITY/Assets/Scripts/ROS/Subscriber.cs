using UnityEngine;

namespace DigitalTwin.ROS {

    /// <summary>
    /// Base class for subscribing to ROS topics. Subclass with a concrete type paramater.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Subscriber<T> : MonoBehaviour where T : Message, new() {
        /// <summary>
        /// Topic name to subscribe to.
        /// </summary>
        public string topic;

        private Bridge bridge;

        /// <summary>
        /// Callback when a message is received.
        /// </summary>
        /// <param name="message"></param>
        public abstract void MessageReceived(T message);

        internal void MessageReceived(Message message) {
            MessageReceived((T)message);
        }

        /// <summary>
        /// Subscribe to a message.
        /// </summary>
        /// <param name="type">Should match the ROS name of the message type.</param>
        public void Subscribe() {
            if(bridge == null) {
                bridge = FindObjectOfType<Bridge>();
            }
            bridge.Subscribe<T>(topic, new T().GetMessageType(), new Bridge.Callback(MessageReceived));
        }

        /// <summary>
        /// Unsubscribe from the topic.
        /// </summary>
        public void Unsubscribe() {
            if(bridge == null) {
                bridge = FindObjectOfType<Bridge>();
            }
            bridge.Unsubscribe(topic);
        }
    }

}