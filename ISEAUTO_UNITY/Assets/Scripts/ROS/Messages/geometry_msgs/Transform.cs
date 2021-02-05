namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Transform : Message {
        public override string GetMessageType() => "geometry_msgs/Transform";

        public Vector3 translation;
        public Quaternion rotation;
    }
}