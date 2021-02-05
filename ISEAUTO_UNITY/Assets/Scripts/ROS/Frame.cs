using UnityEngine;

namespace DigitalTwin.ROS {

    public class Frame {
        public static Vector3 RosToUnity(geometry_msgs.Vector3 vector) {
            return new Vector3((float)-vector.y, (float)vector.z, (float)vector.x);
        }

        public static Vector3 RosToUnity(geometry_msgs.Point point) {
            return new Vector3((float)-point.y, (float)point.z, (float)point.x);
        }

        public static Vector3 RosToUnityAxial(geometry_msgs.Vector3 vector) {
            return new Vector3((float)vector.y, (float)-vector.z, (float)-vector.x);
        }

        public static Quaternion RosToUnity(geometry_msgs.Quaternion quaternion) {
            return new Quaternion((float)quaternion.y, -(float)quaternion.z, -(float)quaternion.x, (float)quaternion.w);
        }

        public static geometry_msgs.Vector3 UnityToRos(Vector3 vector) {
            return new geometry_msgs.Vector3 { x = vector.z, y = -vector.x, z = vector.y };
        }

        public static geometry_msgs.Vector3 UnityToRosAxial(Vector3 vector) {
            return new geometry_msgs.Vector3 { x = -vector.z, y = vector.x, z = -vector.y };
        }

        public static geometry_msgs.Point UnityToRosPoint(Vector3 vector) {
            return new geometry_msgs.Point { x = vector.z, y = -vector.x, z = vector.y };
        }

        public static geometry_msgs.Quaternion UnityToRos(Quaternion quaternion) {
            return new geometry_msgs.Quaternion {
                x = -quaternion.z,
                y = quaternion.x,
                z = -quaternion.y,
                w = quaternion.w
            };
        }
    }
}