using UnityEngine;

namespace DigitalTwin.TrackedController {

    [RequireComponent(typeof(CommandInput), typeof(Rigidbody))]
    public class VirtualVehicleController : MonoBehaviour {
        private CommandInput command;
        private Rigidbody rb;

        private ECU ecu;

        private void FixedUpdate() {
            float forward = Input.GetAxis("Vertical");
            float turn = Input.GetAxis("Horizontal");

            command.forward = forward;
            command.turn = turn;

            if(forward == 0 && turn == 0) {
                rb.isKinematic = true;
                ecu.forwardPID.Reset();
                ecu.turnPID.Reset();
            } else {
                rb.isKinematic = false;
            }
        }

        void Awake() {
            command = GetComponent<CommandInput>();
            rb = GetComponent<Rigidbody>();
            ecu = GetComponent<ECU>();
        }
    }
}