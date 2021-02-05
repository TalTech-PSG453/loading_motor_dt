namespace DigitalTwin.ROS.std_msgs {

    [System.Serializable]
    public class MultiArrayLayout : Message {
        public override string GetMessageType() => "std_msgs/MultiArrayLayout";

        public MultiArrayDimension[] dim;
        public uint data_offset;
    }
}