using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalTwin.ROS;
using DigitalTwin.ROS.std_msgs;
using DigitalTwin.TrackedController;

public class RightTorqueSubscriber : Subscriber<Float32> {

    private TrackedVehicleSimulator sim;

    private void Start() {
        sim = GetComponent<TrackedVehicleSimulator>();
        Subscribe();
    }

    public override void MessageReceived(Float32 message) {
        sim.rightMotor = message.data;
    }
}
