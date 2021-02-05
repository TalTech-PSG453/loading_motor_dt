namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class TwistWithCovariance : Message {
        public override string GetMessageType() => "geometry_msgs/TwistWithCovariance";

        public Twist twist;
        public double[] covariance = new double[36];
    }
}