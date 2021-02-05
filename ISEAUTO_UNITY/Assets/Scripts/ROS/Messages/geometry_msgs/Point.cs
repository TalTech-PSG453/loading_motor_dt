namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Point : Message {
        public override string GetMessageType() => "geometry_msgs/Point";
        public double x;
        public double y;
        public double z;
    }
}