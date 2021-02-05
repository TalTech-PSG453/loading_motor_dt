using DigitalTwin.ROS.geometry_msgs;
using DigitalTwin.Utils;
using UnityEngine;

namespace DigitalTwin.ROS {

    [RequireComponent(typeof(CommandInput))]
    public class CmdVelController : Subscriber<Twist> {
        public float linearNoise = 0f;
        public float angularNoise = 0f;

        private CommandInput command;
        private ECU ecu;

        private void Awake() {
            command = GetComponent<CommandInput>();
            ecu = GetComponent<ECU>();
        }

        private void Start() {
            Subscribe();
        }

        public override void MessageReceived(Twist message) {
            command.forward = ((float)message.linear.x + Noise.ScalarNoise(linearNoise, NoiseType.Gaussian))/ ecu.maxSpeed;
            command.turn = (-(float)message.angular.z + Noise.ScalarNoise(angularNoise, NoiseType.Gaussian)) / ecu.maxAngular;
        }
    }
}