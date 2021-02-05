namespace DigitalTwin.ROS.sensor_msgs
{
    [System.Serializable]
    public class NavSatFix : Message
    {
        public override string GetMessageType() => "sensor_msgs/NavSatFix";

        public const byte COVARIANCE_TYPE_UNKNOWN = 0;
        public const byte COVARIANCE_TYPE_APPROXIMATED = 1;
        public const byte COVARIANCE_TYPE_DIAGONAL_KNOWN = 2;
        public const byte COVARIANCE_TYPE_KNOWN = 3;

        public std_msgs.Header header;
        public NavSatStatus status = new NavSatStatus();
        public double latitude;
        public double longitude;
        public double altitude;
        public double[] position_covariance = new double[9];
        public byte position_covariance_type;
    }
}
