using UnityEngine;

namespace DigitalTwin.Interface {

    public class CommonControls : MonoBehaviour {
        public GameObject helpPanel;
        public GameObject quitDialog;

        void Update() {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Pause();
                quitDialog.SetActive(true);
            }
            if(Input.GetKeyDown(KeyCode.F1)) {
                if(helpPanel.activeSelf) {
                    Unpause();
                    helpPanel.SetActive(false);
                } else {
                    Pause();
                    helpPanel.SetActive(true);
                }
            }
        }

        public void Pause() {
            Time.timeScale = 0f;
        }

        public void Unpause() {
            Time.timeScale = 1f;
        }
    }
}