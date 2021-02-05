namespace DigitalTwin.ROS.std_msgs {
    [System.Serializable]
    public class MultiArrayDimension : Message {
        public override string GetMessageType() => "std_msgs/MultiArrayDimension";

        public string label;
        public uint size;
        public uint stride;
    }
}