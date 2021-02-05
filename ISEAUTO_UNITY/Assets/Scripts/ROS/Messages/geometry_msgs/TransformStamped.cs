namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class TransformStamped : Message {
        public override string GetMessageType() => "geometry_msgs/TransformStamped";

        public ROS.std_msgs.Header header;
        public string child_frame_id;
        public Transform transform;
    }
}