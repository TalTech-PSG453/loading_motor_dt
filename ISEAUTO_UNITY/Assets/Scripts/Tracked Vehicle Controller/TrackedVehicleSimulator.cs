using UnityEngine;

namespace DigitalTwin.TrackedController {

    [RequireComponent(typeof(Rigidbody))]
    public class TrackedVehicleSimulator : MonoBehaviour {
        public float parkingBreakTorque = 1000f;
        public float sprocketRadius = 0.1f;
        public float transmission = 1.2f;
        public float maxMotor = 1000f;
        public Vector3 centerOfMass;

        private WheelCollider[] leftColliders;
        private WheelCollider[] rightColliders;

        [HideInInspector]
        public float leftMotor, rightMotor;
        private float currentLeftMotor, currentRightMotor;

        private Rigidbody rb;
        private Vector3 previousVelocity;

        public bool RegenerativeBreaking {
            get; set;
        }

        public bool ParkingBreak {
            get; set;
        }

        private float baseDamping;

        private void Awake() {
            Utils.Log.screenLogLevel = Utils.LogLevel.INFO;

            leftColliders = transform.Find("LeftColliders").GetComponentsInChildren<WheelCollider>();
            rightColliders = transform.Find("RightColliders").GetComponentsInChildren<WheelCollider>();

            baseDamping = leftColliders[0].wheelDampingRate;
        }

        private void Start() {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = centerOfMass;
        }

        private void FixedUpdate() {
            leftMotor = Mathf.Clamp(leftMotor, -1f, 1f);
            rightMotor = Mathf.Clamp(rightMotor, -1f, 1f);

            foreach(var wheel in leftColliders) {
                DriveWheel(wheel, leftMotor * maxMotor);
            }

            foreach(var wheel in rightColliders) {
                DriveWheel(wheel, rightMotor * maxMotor);
            }

        }

        private void DriveWheel(WheelCollider wheel, float torque) {
            wheel.motorTorque = torque;
            if(ParkingBreak) {
                wheel.brakeTorque = parkingBreakTorque;
            } else {
                wheel.brakeTorque = 0;
            }
        }

        private float AverageRPM(WheelCollider[] colliders) {
            float rpm = 0f;
            int groundedWheels = 0;
            foreach(var wheel in colliders) {
                if(wheel.isGrounded) {
                    groundedWheels++;
                    rpm += wheel.rpm;
                }
            }
            if(groundedWheels > 0) {
                rpm /= groundedWheels;
            }
            return rpm;
        }

        public float WheelToMotorRPM(float rpm) {
            return rpm * WheelRadius() / sprocketRadius * transmission;
        }

        public float LeftRPM() {
            return AverageRPM(leftColliders);
        }

        public float RightRPM() {
            return AverageRPM(rightColliders);
        }

        public float WheelRadius() {
            return leftColliders[0].radius;
        }

        public float Width() {
            return rightColliders[0].transform.parent.localPosition.x - leftColliders[0].transform.parent.localPosition.x;
        }

        public float LeftSpeed() {
            return LeftRPM() * 2 * Mathf.PI * WheelRadius() / 60f;
        }

        public float RightSpeed() {
            return RightRPM() * 2 * Mathf.PI * WheelRadius() / 60f;
        }

        public bool IsGrounded {
            get {
                foreach(var wheel in leftColliders) {
                    if(wheel.isGrounded) {
                        return true;
                    }
                }
                foreach(var wheel in rightColliders) {
                    if(wheel.isGrounded) {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}