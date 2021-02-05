namespace DigitalTwin.ROS.std_msgs {

    [System.Serializable]
    public class Int32 : Message
    {
        public override string GetMessageType() => "std_msgs/Int32";

        public int data;
    }
}
