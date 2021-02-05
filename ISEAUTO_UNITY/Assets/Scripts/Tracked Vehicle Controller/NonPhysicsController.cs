using DigitalTwin.Utils;
using UnityEngine;

namespace DigitalTwin.TrackedController {

    public class NonPhysicsController : MonoBehaviour {
        public float translationSpeed = 10f;
        public float rotationSpeed = 50f;

        private Rigidbody rb;
        /// <summary>
        /// Save the position and rotation of the vehicle in player preferences in slot n.
        /// </summary>
        /// <param name="n"></param>
        private void SavePosition(int n) {
            Vector3 pos = transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
            PlayerPrefs.SetFloat(gameObject.name + "_posX_" + n, pos.x);
            PlayerPrefs.SetFloat(gameObject.name + "_posY_" + n, pos.y);
            PlayerPrefs.SetFloat(gameObject.name + "_posZ_" + n, pos.z);
            PlayerPrefs.SetFloat(gameObject.name + "_rotX_" + n, rot.x);
            PlayerPrefs.SetFloat(gameObject.name + "_rotY_" + n, rot.y);
            PlayerPrefs.SetFloat(gameObject.name + "_rotZ_" + n, rot.z);

            Log.Info("Saved vehicle position in slot " + n);
        }

        /// <summary>
        /// Load the position and rotation of the vehicle from player preferences from slot n. If there is no data in the slot, the vehicle will not move.
        /// </summary>
        /// <param name="n"></param>
        private void LoadPosition(int n) {
            if(!PlayerPrefs.HasKey(gameObject.name + "_posX_" + n)) {
                return;
            }

            Vector3 pos = new Vector3();
            Vector3 rot = new Vector3();
            pos.x = PlayerPrefs.GetFloat(gameObject.name + "_posX_" + n);
            pos.y = PlayerPrefs.GetFloat(gameObject.name + "_posY_" + n);
            pos.z = PlayerPrefs.GetFloat(gameObject.name + "_posZ_" + n);
            rot.x = PlayerPrefs.GetFloat(gameObject.name + "_rotX_" + n);
            rot.y = PlayerPrefs.GetFloat(gameObject.name + "_rotY_" + n);
            rot.z = PlayerPrefs.GetFloat(gameObject.name + "_rotZ_" + n);
            transform.position = pos;
            transform.rotation = Quaternion.Euler(rot);

            Log.Info("Loaded vehicle position from slot " + n);
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) {
                TogglePhysics();
            }

            for(int i = (int)KeyCode.F2, n = 2; i < (int)KeyCode.F12; i++, n++) {
                if(Input.GetKeyDown((KeyCode)i)) {
                    if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                        SavePosition(n);
                    } else {
                        LoadPosition(n);
                    }
                }
            }

            if(rb.isKinematic) {
                Vector3 pos = transform.position;
                Vector3 rot = transform.rotation.eulerAngles;

                pos += transform.forward * translationSpeed * Time.deltaTime * Input.GetAxis("Vertical");
                rot.y += rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal");

                if(Input.GetKeyDown(KeyCode.U)) {
                    rot = new Vector3(0, rot.y, 0);
                }
                if(Input.GetKey(KeyCode.E)) {
                    pos += transform.right * translationSpeed * Time.deltaTime;
                }
                if(Input.GetKey(KeyCode.Q)) {
                    pos -= transform.right * translationSpeed * Time.deltaTime;
                }
                if(Input.GetKey(KeyCode.Z)) {
                    pos -= transform.up * translationSpeed * Time.deltaTime;
                }
                if(Input.GetKey(KeyCode.X)) {
                    pos += transform.up * translationSpeed * Time.deltaTime;
                }

                transform.position = pos;
                transform.rotation = Quaternion.Euler(rot);
            }
        }

        private void TogglePhysics() {
            rb.isKinematic = !rb.isKinematic;

            if(rb.isKinematic) {
                Log.Info("Physics off");
            } else {
                Log.Info("Physics on");
            }
        }

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }
    }
}