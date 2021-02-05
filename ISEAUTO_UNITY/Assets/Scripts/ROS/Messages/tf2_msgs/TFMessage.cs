namespace DigitalTwin.ROS.tf2_msgs {

    [System.Serializable]
    public class TFMessage : Message {
        public override string GetMessageType() => "tf2_msgs/TFMessage";

        public geometry_msgs.TransformStamped[] transforms;
    }
}