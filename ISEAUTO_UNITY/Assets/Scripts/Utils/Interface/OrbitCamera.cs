using UnityEngine;

namespace DigitalTwin.Interface {

    public class OrbitCamera : MonoBehaviour {
        public float rotationSpeed = 1f;

        private Vector3 prevMousePosition;

        private void Start() {
            prevMousePosition = Input.mousePosition;
        }

        private void Update() {
            float deltaX = 0;
            float deltaY = 0;

            if(Input.GetMouseButton(1)) {
                deltaX = Input.mousePosition.x - prevMousePosition.x;
                deltaY = Input.mousePosition.y - prevMousePosition.y;
            }

            prevMousePosition = Input.mousePosition;

            transform.RotateAround(transform.parent.position, Vector3.up, deltaX * Time.deltaTime * rotationSpeed);
            transform.RotateAround(transform.parent.position, transform.right, -deltaY * Time.deltaTime * rotationSpeed);
        }
    }

}