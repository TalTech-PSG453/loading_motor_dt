using UnityEngine;

namespace DigitalTwin.Interface {

    public class FloatingCamera : MonoBehaviour {
        public float translationSpeed = 0.1f;
        public float zoomSpeed = 1f;
        public float rotationSpeed = 1f;

        private Vector3 previousMousePosition;

        private void Start() {
            previousMousePosition = Input.mousePosition;
        }

        private void Update() {
            var pos = transform.localPosition;
            var mousePosition = Input.mousePosition;
            var mouseMovement = mousePosition - previousMousePosition;

            if(Input.GetMouseButton(2)) {
                var translation = -transform.right * mouseMovement.x - transform.up * mouseMovement.y;
                pos += translation * translationSpeed;
            }
            pos += transform.forward * Input.mouseScrollDelta.y * zoomSpeed;

            if(Input.GetMouseButton(1)) {
                transform.rotation = Quaternion.AngleAxis(-mouseMovement.y, transform.right) * transform.rotation;
                transform.rotation = Quaternion.AngleAxis(mouseMovement.x, Vector3.up) * transform.rotation;
            }

            previousMousePosition = mousePosition;
            transform.localPosition = pos;
        }
    }
}