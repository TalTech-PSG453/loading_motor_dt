namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Quaternion : Message {
        public override string GetMessageType() => "geometry_msgs/Quaternion";
        public double x;
        public double y;
        public double z;
        public double w;
    }
}