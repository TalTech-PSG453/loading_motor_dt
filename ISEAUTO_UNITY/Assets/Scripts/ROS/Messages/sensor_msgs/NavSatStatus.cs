namespace DigitalTwin.ROS.sensor_msgs
{
    [System.Serializable]
    public class NavSatStatus : Message
    {
        public override string GetMessageType() => "";

        public const sbyte STATUS_NO_FIX = -1;
        public const sbyte STATUS_FIX = 0;
        public const sbyte STATUS_SBAS_FIX = 1;
        public const sbyte STATUS_GBAS_FIX = 2;

        public sbyte status;

        public const ushort SERVICE_GPS = 1;
        public const ushort SERVICE_GLONASS = 2;
        public const ushort SERVICE_COMPASS = 4;
        public const ushort SERVICE_GALILEO = 8;

        public ushort service;
    }
}
