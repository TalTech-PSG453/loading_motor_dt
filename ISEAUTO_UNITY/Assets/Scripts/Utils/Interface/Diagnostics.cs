using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalTwin.Utils;

public class Diagnostics : MonoBehaviour
{
    private Rigidbody rb;
    private WheelCollider wheel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheel = GetComponentInChildren<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float c = wheel.radius * 2 * Mathf.PI;
        float wheelSpeed = wheel.rpm / 60 * c;
        float speed = rb.velocity.magnitude;
        float expectedRPM = speed / c * 60;
        float slip = (wheelSpeed - speed) / wheelSpeed;
        wheel.GetGroundHit(out WheelHit hit);
        Log.Info("RPM: " + wheel.rpm + " Expected RPM: " + expectedRPM + " calculated slip: " + slip + " slip: " + hit.forwardSlip + " force: " + hit.force);
    }
}
