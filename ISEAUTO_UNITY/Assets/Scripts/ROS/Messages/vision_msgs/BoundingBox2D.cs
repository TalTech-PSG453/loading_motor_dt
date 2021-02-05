namespace DigitalTwin.ROS.vision_msgs {

    [System.Serializable]
    public class BoundingBox2D : Message {
        public override string GetMessageType() => "vision_msgs/BoundingBox2D";
        public geometry_msgs.Pose2D center;
        public double size_x;
        public double size_y;
    }
}