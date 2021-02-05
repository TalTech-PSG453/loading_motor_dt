namespace DigitalTwin.ROS {

    [System.Serializable]
    public class Tag : Message {
        public override string GetMessageType() => "rf_tracking_tags/Tag";

        public std_msgs.Header header;
        public string anchor_addr;
        public string tag_addr;
        public float dist_to_anchor;
        public int tag_timestamp;
    }
}