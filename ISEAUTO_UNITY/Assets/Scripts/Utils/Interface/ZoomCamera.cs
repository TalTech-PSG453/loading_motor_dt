using UnityEngine;

namespace DigitalTwin.Interface {

    public class ZoomCamera : MonoBehaviour {
        public float zoomSpeed = 1f;

        private void Update() {
            var pos = transform.position;
            pos += (Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Zoom") ) * transform.forward * zoomSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }

}