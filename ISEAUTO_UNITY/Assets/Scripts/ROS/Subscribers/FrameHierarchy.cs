using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwin.ROS {

    public class FrameHierarchy : Subscriber<tf2_msgs.TFMessage> {
        private Dictionary<string, GameObject> frameObjects = new Dictionary<string, GameObject>();
        private geometry_msgs.TransformStamped[] transforms;

        public string baseLinkId;
        public Vector3 baseLinkPosition = Vector3.zero;
        public Dictionary<GameObject, string> linkedObjects;

        public override void MessageReceived(tf2_msgs.TFMessage message) {
            transforms = message.transforms;
        }

        private void Update() {
            if(transforms != null) {
                foreach(var tf in transforms) {
                    if(!frameObjects.ContainsKey(tf.header.frame_id)) {
                        CreateObject(tf.header.frame_id, Vector3.zero, Quaternion.identity);
                    }
                    if(!frameObjects.ContainsKey(tf.child_frame_id)) {
                        CreateObject(tf.child_frame_id, Frame.RosToUnity(tf.transform.translation), Frame.RosToUnity(tf.transform.rotation));
                    } else { // Object already created earlier, need to set its pose now
                        var obj = frameObjects[tf.child_frame_id];
                        obj.transform.localPosition = Frame.RosToUnity(tf.transform.translation);
                        obj.transform.localRotation = Frame.RosToUnity(tf.transform.rotation);
                    }

                    var parent = frameObjects[tf.header.frame_id];
                    var child = frameObjects[tf.child_frame_id];

                    child.transform.SetParent(parent.transform, false);
                }

                var base_link = frameObjects[baseLinkId];
                base_link.transform.SetParent(this.transform, false);
                base_link.transform.localPosition = baseLinkPosition;

                if(linkedObjects != null) {
                    LinkObjects();
                }
                transforms = null;
            }
        }

        private void LinkObjects() {
            foreach(var obj in linkedObjects.Keys) {
                obj.transform.SetParent(frameObjects[linkedObjects[obj]].transform, false);
                
            }
        }

        private void CreateObject(string frame_id, UnityEngine.Vector3 translation, UnityEngine.Quaternion rotation) {
            var go = new GameObject();
            frameObjects.Add(frame_id, go);
            go.name = frame_id;

            go.transform.localPosition = translation;
            go.transform.localRotation = rotation;
        }

        private void Start() {
            Subscribe();
        }
    }

}