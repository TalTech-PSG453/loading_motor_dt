using UnityEngine;

namespace DigitalTwin.Interface {

    public class QuitDialog : MonoBehaviour {
        private CommonControls commonControls;

        private void Start() {
            commonControls = transform.parent.GetComponent<CommonControls>();
        }

        public void QuitAnswerYes() {
            Application.Quit();
        }

        public void QuitAnswerNo() {
            gameObject.SetActive(false);
            commonControls.Unpause();
        }
    }
}