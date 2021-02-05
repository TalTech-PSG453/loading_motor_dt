using System.Collections.Generic;
using DigitalTwin.Utils;
using UnityEngine;

namespace DigitalTwin.TrackedController {

    /// <summary>
    /// Input lag manager. Tracks axis input and lags it by the specified amount. Forwards non-lagging axes directly to the Input class.
    /// </summary>
    public class InputLag : MonoBehaviour {
        public string[] laggingAxes;
        public float lag;
        public float lagNoise;

        public float CurrentLag {
            get; private set;
        }

        private struct InputEvent {
            public float timestamp;
            public float value;

            public InputEvent(float timestamp, float value) {
                this.timestamp = timestamp;
                this.value = value;
            }
        }

        private Dictionary<string, Queue<InputEvent>> buffers;
        private Dictionary<string, float> currentValue;
        private Pool<InputEvent> eventPool;

        /// <summary>
        /// Get axis value, lagged or unlagged, depending on the setup of this component.
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetAxis(string axis) {
            if(currentValue.ContainsKey(axis)) {
                return currentValue[axis];
            } else {
                return Input.GetAxis(axis);
            }
        }

        private void Start() {
            eventPool = new Pool<InputEvent>();
            buffers = new Dictionary<string, Queue<InputEvent>>();
            currentValue = new Dictionary<string, float>();
            foreach(var axis in laggingAxes) {
                buffers.Add(axis, new Queue<InputEvent>());
                currentValue.Add(axis, 0);
            }
        }

        private void FixedUpdate() {
            CurrentLag = lag + Noise.ScalarNoise(lagNoise, NoiseType.Uniform);
            foreach(var axis in laggingAxes) {
                var buffer = buffers[axis];
                var inputEvent = eventPool.Get();
                inputEvent.timestamp = Time.time;
                inputEvent.value = Input.GetAxis(axis);
                buffer.Enqueue(inputEvent);

                var firstEvent = buffer.Peek();
                while(firstEvent.timestamp + CurrentLag <= Time.time) {
                    currentValue[axis] = firstEvent.value;
                    eventPool.Free(buffer.Dequeue());
                    if(buffer.Count > 0) {
                        firstEvent = buffer.Peek();
                    } else {
                        break;
                    }
                }
            }
        }
    }
}