namespace DigitalTwin.ROS.std_msgs {

    [System.Serializable]
    public class Float32 : Message {
        public override string GetMessageType() => "std_msgs/Float32";

        public float data;
    }
}