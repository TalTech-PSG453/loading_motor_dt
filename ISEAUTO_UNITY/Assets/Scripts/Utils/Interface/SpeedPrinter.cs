using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalTwin.TrackedController;
using DigitalTwin.Utils;

public class SpeedPrinter : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

        Log.screenLogLevel = LogLevel.INFO;
        Log.consoleLogLevel = LogLevel.NONE;
    }

    private void Update() {
        float speed = transform.InverseTransformDirection(rb.velocity).z;
        float angular = rb.angularVelocity.y;

        Log.Info("Speed: " + speed + " m/s; Angular: " + angular + " rad/s");
    }
}
