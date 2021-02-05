using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwin.ROS {

    [RequireComponent(typeof(Rigidbody))]
    public class UTMPublisher : Publisher<nav_msgs.Odometry> {
        public const double metersPerDegree = 111111;

        public float frequency = 50.0f;
        public float positionNoise = 0.0f;
        public float rotationNoise = 0.0f;
        public float velocityNoise = 0.0f;

        public float originLatitude = 58.382206f;
        public float originLongitude = 26.730512f;
        public float originAltitude = 38f;

        private const double E0 = 500000;
        private const double N0 = 10000000;

        private double originE;
        private double originN;

        [HideInInspector]
        public string frame;

        [HideInInspector]
        public string childFrame;

        private Rigidbody rb;

        private float delay;

        // Start is called before the first frame update
        void Start() {
            if(originLatitude >= 0) {
                originN = originLatitude * metersPerDegree;
            } else {
                originN = N0 + originLatitude * metersPerDegree;
            }

            int zone = (int)(originLongitude / 6);
            double degreesFromCenter = originLongitude - (zone * 6 + 3);
            originE = E0 + degreesFromCenter * metersPerDegree * Mathf.Cos(originLatitude * Mathf.Deg2Rad);

            delay = 1.0f / frequency;

            rb = GetComponent<Rigidbody>();

            Advertise();
        }

        private float time;

        private void FixedUpdate() {
            time += Time.deltaTime;
            if(time >= delay) {
                time -= delay;

                var pos = transform.position + Utils.Noise.Vector3Noise(positionNoise, Utils.NoiseType.Gaussian);
                var rot = transform.rotation * Quaternion.Euler(Utils.Noise.Vector3Noise(rotationNoise, Utils.NoiseType.Gaussian));

                var linearTwist = transform.InverseTransformDirection(rb.velocity).z + Utils.Noise.ScalarNoise(velocityNoise, Utils.NoiseType.Gaussian);
                var angularTwist = -transform.InverseTransformDirection(rb.angularVelocity).y + Utils.Noise.ScalarNoise(velocityNoise, Utils.NoiseType.Gaussian);
                var rosPos = Frame.UnityToRosPoint(pos);
                rosPos.x += originE;
                rosPos.y += originN;
                var rosRot = Frame.UnityToRos(rot);
                var msg = new nav_msgs.Odometry {
                    header = new std_msgs.Header {
                        frame_id = frame,
                        stamp = std_msgs.TimeStamp.CurrentTimeStamp()
                    },
                    child_frame_id = childFrame,
                    pose = new geometry_msgs.PoseWithCovariance {
                        pose = new geometry_msgs.Pose {
                            position = rosPos,
                            orientation = rosRot
                        },
                        covariance = new double[] {
                    positionNoise*positionNoise, 0, 0, 0, 0, 0,
                    0, positionNoise*positionNoise, 0, 0, 0, 0,
                    0, 0, positionNoise*positionNoise, 0, 0, 0,
                    0, 0, 0, rotationNoise*rotationNoise, 0, 0,
                    0, 0, 0, 0, rotationNoise*rotationNoise, 0,
                    0, 0, 0, 0, 0, rotationNoise*rotationNoise
                }
                    },
                    twist = new geometry_msgs.TwistWithCovariance {
                        twist = new geometry_msgs.Twist {
                            linear = new geometry_msgs.Vector3 {
                                x = linearTwist,
                                y = 0,
                                z = 0
                            },
                            angular = new geometry_msgs.Vector3 {
                                x = 0,
                                y = 0,
                                z = angularTwist
                            }
                        },
                        covariance = new double[] {
                    velocityNoise*velocityNoise, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, velocityNoise*velocityNoise
                }
                    }
                };

                Publish(msg);
            }
        }
    }
}