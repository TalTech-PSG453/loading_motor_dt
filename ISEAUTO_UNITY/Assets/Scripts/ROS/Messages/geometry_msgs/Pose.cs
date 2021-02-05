namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Pose : Message {
        public override string GetMessageType() => "geometry_msgs/Pose";
        public Point position;
        public Quaternion orientation;
    }
}