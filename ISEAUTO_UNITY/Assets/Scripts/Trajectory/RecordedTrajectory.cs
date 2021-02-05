using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwin.Trajectory {

    public class RecordedTrajectory : MonoBehaviour {
        public struct Pose {
            public Vector3 position;
            public Vector3 velocity;
        }

        public float tolerance;
        public GameObject virtualVehicle;
        public System.Action queueUpdated;
        public int GoalIndex {
            get; private set;
        }

        private LineRenderer lr;

        public List<Pose> points = new List<Pose>();

        private Vector3 lastPosition = Vector3.zero;

        private void Awake() {
            lr = GetComponentInChildren<LineRenderer>();
        }

        private void Update() {
            var virtualPosition = virtualVehicle.transform.position;
            if(Vector3.Distance(virtualPosition, lastPosition) > tolerance) {
                Pose pose = new Pose {
                    position = virtualVehicle.transform.position,
                    velocity = virtualVehicle.transform.InverseTransformVector(virtualVehicle.GetComponent<Rigidbody>().velocity)
                };
                points.Add(pose);
                queueUpdated?.Invoke();
                lastPosition = virtualPosition;

                lr.positionCount = points.Count;
                lr.SetPositions(PoseToPositions());
            }

            while(points.Count > 0 && GoalIndex < points.Count-1 && Vector3.Distance(transform.position, points[GoalIndex].position) < tolerance) {
                GoalIndex++;
            }
        }

        private Vector3[] PoseToPositions() {
            Vector3[] array = new Vector3[points.Count];
            for(int i = 0; i < points.Count; i++) {
                array[i] = points[i].position;
            }
            return array;
        }
    }
}