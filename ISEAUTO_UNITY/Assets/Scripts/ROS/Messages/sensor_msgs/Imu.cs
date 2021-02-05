namespace DigitalTwin.ROS.sensor_msgs {

    [System.Serializable]
    public class Imu : Message {
        public override string GetMessageType() => "sensor_msgs/Imu";

        public std_msgs.Header header;
        public geometry_msgs.Quaternion orientation;
        public double[] orientation_covariance = new double[9];
        public geometry_msgs.Vector3 angular_velocity;
        public double[] angular_velocity_covariance = new double[9];
        public geometry_msgs.Vector3 linear_acceleration;
        public double[] linear_acceleration_covariance = new double[0];
    }
}
