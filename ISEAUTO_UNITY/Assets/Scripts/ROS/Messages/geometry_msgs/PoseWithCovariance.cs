namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class PoseWithCovariance : Message {
        public override string GetMessageType() => "geometry_msgs/PoseWithCovariance";

        public Pose pose;
        public double[] covariance = new double[36];
    }
}