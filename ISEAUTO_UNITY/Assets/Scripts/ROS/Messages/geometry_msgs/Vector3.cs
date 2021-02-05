namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class Vector3 : Message {
        public override string GetMessageType() => "geometry_msgs/Vector3";
        public double x;
        public double y;
        public double z;
    }
}