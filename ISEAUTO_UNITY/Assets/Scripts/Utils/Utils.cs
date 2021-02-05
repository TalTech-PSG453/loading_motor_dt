using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DigitalTwin.Utils
{
    public static class Utils
    {
        public static bool IsEditingInputField()
        {
            return EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null;
        }
    }
}