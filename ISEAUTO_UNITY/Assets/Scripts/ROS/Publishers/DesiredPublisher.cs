using DigitalTwin.ROS.std_msgs;
using Unity.Collections;
using UnityEngine;

namespace DigitalTwin.ROS {

    public class DesiredPublisher : Publisher<std_msgs.Float32>
    {
        public float frequency=50;

        private float time = 0;
        private float delay;

        public float DesiredRPM;
        private void Start() {
            delay = 1.0f / frequency;
           // Advertise();
        }

        private void FixedUpdate() {
            time += Time.deltaTime;
            if(time >= delay) {
                time -= delay;

                var msg = new std_msgs.Float32 {
                    data = DesiredRPM
                };

                Publish(msg);
            }
        }
    }
}