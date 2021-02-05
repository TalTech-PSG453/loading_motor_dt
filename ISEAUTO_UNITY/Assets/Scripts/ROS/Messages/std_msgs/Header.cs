using UnityEngine;

namespace DigitalTwin.ROS.std_msgs {

    
    [System.Serializable]
    public class Header : Message {
        public override string GetMessageType() => "std_msgs/Header";
        public uint seq;
        public TimeStamp stamp;
        public string frame_id;
    }

    [System.Serializable]
    public struct TimeStamp {
        public uint secs;
        public uint nsecs;

        public static TimeStamp CurrentTimeStamp() {
            return new TimeStamp {
                secs = (uint)Time.time,
                nsecs = (uint)((Time.time - (uint)(Time.time)) * 1e9f)
            };
        }
    }

}