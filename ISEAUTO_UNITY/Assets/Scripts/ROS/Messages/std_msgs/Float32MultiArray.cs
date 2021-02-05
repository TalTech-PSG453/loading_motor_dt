namespace DigitalTwin.ROS.std_msgs {

    public class Float32MultiArray : Message {
        public override string GetMessageType() => "std_msgs/Float32MultiArray";

        public MultiArrayLayout layout;
        public float[] data;
    }
}