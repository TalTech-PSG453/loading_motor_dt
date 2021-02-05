using UnityEngine;

namespace DigitalTwin.ROS {

    /// <summary>
    /// Base class for publishers. Subclass with a specific message type parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Publisher<T> : MonoBehaviour where T : Message, new() {
        /// <summary>
        /// Topic to publish.
        /// </summary>
        public string topic;

        private Bridge bridge;

        /// <summary>
        /// Advertise a topic. This must be called before attempting to publish anything.
        /// </summary>
        /// <param name="type">Should match the ROS type name of the message.</param>
        public void Advertise() {
            if(bridge == null) {
                bridge = FindObjectOfType<Bridge>();
            }
            bridge.Advertise(topic, new T().GetMessageType());
        }

        /// <summary>
        /// Unadvertise the topic.
        /// </summary>
        public void Unadvertise() {
            if(bridge == null) {
                bridge = FindObjectOfType<Bridge>();
            }
            bridge.Unadvertise(topic);
        }

        /// <summary>
        /// Publish a message. The topic must be advertised first.
        /// </summary>
        /// <param name="message"></param>
        public void Publish(T message) {
            if(bridge == null) {
                bridge = FindObjectOfType<Bridge>();
            }
            bridge.Publish(topic, message);
        }
    }
}