﻿using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalTwin.ROS;
using DigitalTwin.ROS.std_msgs;
using DigitalTwin.TrackedController;

public class TorqueSubscriber : Subscriber<Float32>
{

    private MotorController controller;

    private void Start() {
        controller = GetComponent<MotorController>();
        
    }


    public override void MessageReceived(Float32 message) {
        controller.torque = message.data;
    }
}
