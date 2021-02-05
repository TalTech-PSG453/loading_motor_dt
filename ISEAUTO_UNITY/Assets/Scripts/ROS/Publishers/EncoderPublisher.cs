using DigitalTwin.ROS.std_msgs;
using Unity.Collections;
using UnityEngine;

namespace DigitalTwin.ROS {

    public class EncoderPublisher : Publisher<std_msgs.Float32>
    {

        public Rigidbody encoder;
        public float frequency=50;

        private float time = 0;
        private float delay;

        public float encoderValue;
        private void Start() {
            delay = 1.0f / frequency;
            Advertise();
        }

        private void FixedUpdate() {
            time += Time.deltaTime;
            if(time >= delay) {
                time -= delay;

                var msg = new std_msgs.Float32 {
                    data = encoder.angularVelocity.x*60/(2*Mathf.PI)
                };

                print("RPM: "+msg.data);
                print("Angular Velocity: "+encoder.angularVelocity.x);
                Publish(msg);
            }
        }
    }
}