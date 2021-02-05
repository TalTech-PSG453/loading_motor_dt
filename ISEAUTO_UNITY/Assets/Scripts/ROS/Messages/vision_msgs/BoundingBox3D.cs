namespace DigitalTwin.ROS.vision_msgs {

    [System.Serializable]
    public class BoundingBox3D : Message {
        public override string GetMessageType() => "vision_msgs/BoundingBox3D";
        public geometry_msgs.Pose center;
        public geometry_msgs.Vector3 size;
    }
}