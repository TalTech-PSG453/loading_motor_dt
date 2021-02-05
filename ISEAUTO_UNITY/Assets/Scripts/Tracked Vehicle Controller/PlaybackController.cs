using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwin.TrackedController {

    [RequireComponent(typeof(CommandInput))]
    public class PlaybackController : MonoBehaviour {
        private CommandInput command;

        private struct InputEvent {
            public float timestamp;
            public float forward, turn;
        }

        private Queue<InputEvent> buffer = new Queue<InputEvent>();

        private float time;

        private void Awake() {
            command = GetComponent<CommandInput>();
        }

        private void FixedUpdate() {
            InputEvent input;
            input.timestamp = Time.time;
            input.forward = Input.GetAxis("Vertical");
            input.turn = Input.GetAxis("Horizontal");
            if(input.forward != 0 || input.turn != 0) {
                buffer.Enqueue(input);
            }

            if(Input.GetKeyDown(KeyCode.Tab)) {
                StartPlayback();
            }
            if(Input.GetKeyUp(KeyCode.Tab)) {
                StopPlayback();
            }
            if(Input.GetKey(KeyCode.Tab)) {
                Playback();
            }
        }

        private void Playback() {
            time += Time.deltaTime;
            while(buffer.Count > 0 && time >= buffer.Peek().timestamp) {
                var inputEvent = buffer.Dequeue();
                command.forward = inputEvent.forward;
                command.turn = inputEvent.turn;
            }
            if(buffer.Count == 0) {
                StopPlayback();
            }
        }

        private void StartPlayback() {
            if(buffer.Count > 0) {
                time = buffer.Peek().timestamp;
            }
        }

        private void StopPlayback() {
            command.forward = 0;
            command.turn = 0;
        }
    }
}