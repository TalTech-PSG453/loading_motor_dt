using UnityEngine;

namespace DigitalTwin.Utils {

    [System.Serializable]
    public class PID {
        public float kP, kI, kD;
        public float feedForward;

        private float previous = 0;
        private float sum = 0;

        public PID(float kP, float kI, float kD, float feedForward = 0) {
            this.kP = kP;
            this.kI = kI;
            this.kD = kD;
            this.feedForward = feedForward;
        }

        public float Value(float diff) {
            sum += diff * Time.deltaTime;

            float difference = (diff - previous) / Time.deltaTime;
            previous = diff;

            return diff * kP + sum * kI + difference * kD;
        }

        public float Value(float target, float input) {
            return Value(target - input) + feedForward * target;
        }

        public void Reset() {
            sum = 0;
        }
    }
}