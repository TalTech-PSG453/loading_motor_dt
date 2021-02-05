using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwin.ROS {

    public class RadioBeaconPublisher : Publisher<Tag> {
        [HideInInspector]
        public string frame;

        public float frequency;
        public float distanceNoise;

        private float time = 0;
        private float delay;

        private GameObject[] tags, anchors;

        public bool packetLossEnabled;
        public float maxLossProbability;
        public float minLossProbability;
        public float maxLossDistance;
        public float minLossDistance;
        public float maxReadingDistance;

        void Start() {
            tags = GameObject.FindGameObjectsWithTag("rf-tag");
            anchors = GameObject.FindGameObjectsWithTag("rf-anchor");

            delay = 1.0f / frequency;
            Advertise();
        }

        private void FixedUpdate() {
            time += Time.deltaTime;
            if(time >= delay) {
                time -= delay;

                for(int anchorCount = 0; anchorCount < anchors.Length; anchorCount++) {
                    for(int tagCount = 0; tagCount < tags.Length; tagCount++) {
                        float distance = Distance(anchors[anchorCount], tags[tagCount]);
                        if(PacketReceived(distance)) {
                            var msg = new Tag {
                                header = new std_msgs.Header {
                                    frame_id = frame,
                                    stamp = std_msgs.TimeStamp.CurrentTimeStamp()
                                },
                                anchor_addr = (anchorCount + 1).ToString("d2"),
                                tag_addr = (tagCount + 1).ToString("d4"),
                                dist_to_anchor = distance + Utils.Noise.ScalarNoise(distanceNoise, Utils.NoiseType.Gaussian),
                                tag_timestamp = (int)(Time.time * 1000)
                            };
                            Publish(msg);
                        }
                    }
                }
            }
        }

        private float Distance(GameObject anchor, GameObject tag) {
            return Vector3.Distance(anchor.transform.position, tag.transform.position);
        }

        private bool PacketReceived(float distance) {
            if(!packetLossEnabled) {
                return true;
            }
            if(distance > maxReadingDistance) {
                return false;
            }
            float t = (distance - minLossDistance) / (maxLossDistance - minLossDistance);
            float lossProbability = Mathf.Clamp(Mathf.Lerp(minLossProbability, maxLossProbability, t), minLossProbability, maxLossProbability);
            return Random.value > lossProbability;
        }
    }
}