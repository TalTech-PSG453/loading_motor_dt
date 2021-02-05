namespace DigitalTwin.ROS {

    public class ClockPublisher : Publisher<rosgraph_msgs.Clock> {
        // Start is called before the first frame update
        void Start() {
            Advertise();
        }

        private void FixedUpdate() {
            var msg = new rosgraph_msgs.Clock {
                clock = std_msgs.TimeStamp.CurrentTimeStamp()
            };
            Publish(msg);
        }
    }
}