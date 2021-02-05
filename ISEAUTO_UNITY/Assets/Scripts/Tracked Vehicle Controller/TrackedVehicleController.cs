using UnityEngine;

namespace DigitalTwin.TrackedController {
    /// <summary>
    /// The class of tank controller
    /// </summary>
    [RequireComponent(typeof(CommandInput))]
    public class TrackedVehicleController : MonoBehaviour {
        private CommandInput command;

        private void FixedUpdate() {
            command.forward = Input.GetAxis("Vertical");
            command.turn = Input.GetAxis("Horizontal");
        }

        void Awake() {
            command = GetComponent<CommandInput>();
        }
    }
}