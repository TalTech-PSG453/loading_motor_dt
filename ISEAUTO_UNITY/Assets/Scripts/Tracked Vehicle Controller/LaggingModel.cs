using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaggingModel : MonoBehaviour
{
    public float delay;
    public Transform vehicle;

    private Queue<Vector3> positions;
    private Queue<Quaternion> rotations;

    private void Start() {
        positions = new Queue<Vector3>();
        rotations = new Queue<Quaternion>();
        int steps = (int)(delay / Time.fixedDeltaTime);
        for(int i = 0; i < steps; i++) {
            positions.Enqueue(transform.position);
            rotations.Enqueue(transform.rotation);
        }
    }

    private void FixedUpdate() {
        transform.position = positions.Dequeue();
        transform.rotation = rotations.Dequeue();
        positions.Enqueue(vehicle.position);
        rotations.Enqueue(vehicle.rotation);
    }
}
