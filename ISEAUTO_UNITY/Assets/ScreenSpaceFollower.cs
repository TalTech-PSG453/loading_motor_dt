namespace Games.NoSoySauce.Outsource.Fox3DMinigames.City5G.Minigames.Targeting
{
    using UnityEngine;

    /// <summary>
    ///     Makes a 2D RectTransform follow a 3D Transform in screen space.
    /// </summary>
    public class ScreenSpaceFollower : MonoBehaviour
    {
        public RectTransform follower;
        public Transform target;
        public bool accountRotation;

        private RectTransform canvasRectTransform;

        private void Reset() { follower = GetComponent<RectTransform>(); }

        private void Start() { canvasRectTransform = follower.GetComponentInParent<RectTransform>(); }

        private void LateUpdate() { Follow(); }

        private void Follow()
        {
            //if (!follower || !target || !followerCanvas) return; - to catch errors

            var camera = Camera.main;
            var screenPosition = camera.WorldToScreenPoint(target.position);
            follower.position = screenPosition;

            if (accountRotation)
            {
                // We assume that target's forward direction projected on the screen is the follower's up direction
                // Calculate direction vector
                var targetPosition = target.position;
                var pointA = camera.WorldToScreenPoint(targetPosition);
                var pointB = camera.WorldToScreenPoint(targetPosition + target.forward);
                var directionOnScreen = (pointB - pointA).normalized;

                // Apply rotation
                var followerRotation = Quaternion.LookRotation(Vector3.forward, directionOnScreen);
                follower.rotation = followerRotation;
            }

            // one more solution: https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html

            /*
            // Reset Z
            var localPosition = follower.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, originalZ);
            follower.localPosition = localPosition;
            var viewportPosition = camera.WorldToViewportPoint(target.position);
            follower.position = new Vector3(viewportPosition.x, viewportPosition.y, follower.localPosition.z);
            */
            /*
             var sizeDelta = canvasRectTransform.sizeDelta;
            follower.anchoredPosition = new Vector2(
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));*/

            //follower.rotation = Camera.main.WorldToScreenPoint(target.position);
        }
    }
}