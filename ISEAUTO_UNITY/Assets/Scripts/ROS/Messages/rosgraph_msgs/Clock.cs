namespace DigitalTwin.ROS.rosgraph_msgs {

    [System.Serializable]
    public class Clock : Message {
        public override string GetMessageType() => "rosgraph_msgs/Clock";

        public std_msgs.TimeStamp clock;
    }
}