using UnityEngine;

namespace DigitalTwin.TrackedController {

    [RequireComponent(typeof(InputLag), typeof(CommandInput))]
    public class LaggingController : MonoBehaviour {
        private InputLag lag;
        private CommandInput command;

        private void FixedUpdate() {
            command.forward = lag.GetAxis("Vertical");
            command.turn = lag.GetAxis("Horizontal");
        }

        void Awake() {
            lag = GetComponent<InputLag>();
            command = GetComponent<CommandInput>();
        }
    }
}