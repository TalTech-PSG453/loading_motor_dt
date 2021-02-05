namespace DigitalTwin.ROS.std_msgs {

    [System.Serializable]
    public class String : Message {
        public override string GetMessageType() => "std_msgs/String";

        public string data;
    }
}