using System.Collections;
using System.Collections.Generic;
using DigitalTwin.ROS.std_msgs;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    public Rigidbody Cage;
    private Vector3 pose;
    private Vector3 rotate;
    public float torque;
    public float maxTorque;
    public ConstantForce MotorForce;
    
    // Start is called before the first frame update
    void Start()
    {
        pose = Cage.position;
        Cage.maxAngularVelocity = Mathf.Infinity;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Cage.position = pose;
        rotate.y = torque;
        MotorForce.relativeTorque = rotate;
    }
}
