namespace DigitalTwin.ROS.nav_msgs {

    [System.Serializable]
    public class Odometry : Message {
        public override string GetMessageType() => "nav_msgs/Odometry";

        public std_msgs.Header header;
        public string child_frame_id;
        public geometry_msgs.PoseWithCovariance pose;
        public geometry_msgs.TwistWithCovariance twist;
    }
}