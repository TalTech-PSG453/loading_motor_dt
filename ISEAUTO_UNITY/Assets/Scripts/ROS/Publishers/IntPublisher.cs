using DigitalTwin.ROS.std_msgs;
using Unity.Collections;
using UnityEngine;

namespace DigitalTwin.ROS {

    public class IntPublisher : Publisher<std_msgs.Int32>
    {

        public int d;
        public float frequency=50;

        private float time = 0;
        private float delay;

        private void Start() {
            delay = 1.0f / frequency;
            Advertise();
        }

        private void FixedUpdate() {
            time += Time.deltaTime;
            if(time >= delay) {
                time -= delay;

                var msg = new std_msgs.Int32 {
                    data = d
                };

                
                Publish(msg);
            }
        }
    }
}