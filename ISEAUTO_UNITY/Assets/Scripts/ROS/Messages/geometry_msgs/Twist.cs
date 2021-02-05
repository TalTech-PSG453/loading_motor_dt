namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Twist : Message {
        public override string GetMessageType() => "geometry_msgs/Twist";
        public Vector3 linear;
        public Vector3 angular;
    }
}