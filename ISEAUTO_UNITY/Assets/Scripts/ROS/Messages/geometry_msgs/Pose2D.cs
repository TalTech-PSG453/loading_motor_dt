namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Pose2D : Message {
        public override string GetMessageType() => "geometry_msgs/Pose2D";
        public double x;
        public double y;
        public double theta;
    }
}