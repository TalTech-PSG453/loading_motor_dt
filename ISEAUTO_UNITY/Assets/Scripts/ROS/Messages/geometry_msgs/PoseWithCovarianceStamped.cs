using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwin.ROS.geometry_msgs {

    [System.Serializable]
    public class PoseWithCovarianceStamped : Message {
        public override string GetMessageType() => "geometry_msgs/PoseWithCovarianceStamped";

        public std_msgs.Header header;
        public PoseWithCovariance pose;
    }
}
