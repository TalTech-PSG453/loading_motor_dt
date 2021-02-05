using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using DigitalTwin.Utils;
using DigitalTwin.TrackedController;

[RequireComponent(typeof(TrackedVehicleSimulator), typeof(Rigidbody), typeof(CommandInput))]
public class ECU : MonoBehaviour
{
    public string left_publish="/unity/left";
    public string right_publish="/unity/right";
    
    public string left_subscribe="/themis/drive/ramped/left";
    public string right_subscribe="/themis/drive/ramped/right";
    
    public PublisherLeftPreRamp leftTempPublisher;
    public PublisherRightPreRamp rightTempPublisher;
    public TorqueSubscriber final;
    public RightTorqueSubscriber rightFinal;
    
    
    
    
    [Header("Speed limits")]
    public float maxSpeed;
    public float maxAngular;
  
    [Header("Acceleration limits")]
    public float maxForwardG;
    public float maxTurnG;
    public float maxBrakingG;

    [Header("Parking brake")]
    public float parkingBrakeSpeedTolerance;

    [Header("PIDs")]
    public PID forwardPID;
    public PID turnPID;


    private Rigidbody rb;
    private TrackedVehicleSimulator sim;
    private CommandInput command;

    private float r;
    private Vector3 previousVelocity;

    private float gravity;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        sim = GetComponent<TrackedVehicleSimulator>();
        command = GetComponent<CommandInput>();
        
        leftTempPublisher=gameObject.AddComponent<PublisherLeftPreRamp>();
        rightTempPublisher=gameObject.AddComponent<PublisherRightPreRamp>();
        leftTempPublisher.topic = left_publish;
        rightTempPublisher.topic = right_publish;
        
        final=gameObject.AddComponent<TorqueSubscriber>();
        rightFinal=gameObject.AddComponent<RightTorqueSubscriber>();
        final.topic = left_subscribe;
        rightFinal.topic = right_subscribe;
        
        
        
        r = sim.Width() * 0.5f;
        gravity = Physics.gravity.magnitude;
    }

    private void FixedUpdate() {
        if(!sim.IsGrounded) {
            forwardPID.Reset();
            turnPID.Reset();
            return;
        }

        /*if(command.forward == 0 && command.turn == 0 && sim.LeftSpeed() <= parkingBrakeSpeedTolerance && sim.RightSpeed() <= parkingBrakeSpeedTolerance) {
            sim.ParkingBreak = true;
            forwardPID.Reset();
            turnPID.Reset();
            return;
        }*/

        sim.ParkingBreak = false;

        float speedSign = Mathf.Sign(Vector3.Dot(rb.velocity, transform.forward));
        float realForwardSpeed = rb.velocity.magnitude * speedSign;
        float realTurnSpeed = rb.angularVelocity.y;
        float maxAngularAtCurrentSpeed = Mathf.Min(maxTurnG * gravity / Mathf.Abs(realForwardSpeed), maxAngular);

        float desiredLeftSpeed = command.forward * maxSpeed + command.turn * maxAngularAtCurrentSpeed * r;
        float desiredRightSpeed = command.forward * maxSpeed - command.turn * maxAngularAtCurrentSpeed * r;

        if(desiredLeftSpeed >= maxSpeed || desiredRightSpeed >= maxSpeed) {
            float diff = Mathf.Max(desiredLeftSpeed, desiredRightSpeed) - maxSpeed;
            desiredLeftSpeed -= diff;
            desiredRightSpeed -= diff;
        } else if(desiredLeftSpeed <= -maxSpeed || desiredRightSpeed <= -maxSpeed) {
            float diff = -Mathf.Min(desiredLeftSpeed, desiredRightSpeed) - maxSpeed;
            desiredLeftSpeed += diff;
            desiredRightSpeed += diff;
        }

        float desiredForwardSpeed = (desiredLeftSpeed + desiredRightSpeed) * 0.5f;
        float desiredTurnSpeed = (desiredLeftSpeed - desiredForwardSpeed) / r;

        float desiredForwardAcceleration = (desiredForwardSpeed - realForwardSpeed) / Time.deltaTime / gravity;

        if(speedSign > 0) {
            if(desiredForwardAcceleration > maxForwardG) {
                desiredForwardSpeed = realForwardSpeed + maxForwardG * gravity * Time.deltaTime;
            } else if(desiredForwardAcceleration < -maxBrakingG) {
                desiredForwardSpeed = realForwardSpeed - maxBrakingG * gravity * Time.deltaTime;
            }
        } else {
            if(desiredForwardAcceleration < -maxForwardG) {
                desiredForwardSpeed = realForwardSpeed - maxForwardG * gravity * Time.deltaTime;
            } else if(desiredForwardAcceleration > maxBrakingG) {
                desiredForwardSpeed = realForwardSpeed + maxBrakingG * gravity * Time.deltaTime;
            }
        }

        float forward = forwardPID.Value(desiredForwardSpeed, realForwardSpeed);
        float turn = turnPID.Value(desiredTurnSpeed, realTurnSpeed);

        float leftMotor = forward + turn;
        float rightMotor = forward - turn;

        if(leftMotor > 1 && leftMotor > rightMotor) {
            float ratio = leftMotor / rightMotor;
            leftMotor = 1f;
            rightMotor = 1f / ratio;
        } else if(rightMotor > 1 && rightMotor > leftMotor) {
            float ratio = rightMotor / leftMotor;
            rightMotor = 1f;
            leftMotor = 1f / ratio;
        } else if(leftMotor < -1 && leftMotor < rightMotor) {
            float ratio = leftMotor / rightMotor;
            leftMotor = -1f;
            rightMotor = -1f / ratio;
        } else if(rightMotor < -1 && rightMotor < leftMotor) {
            float ratio = rightMotor / leftMotor;
            rightMotor = -1f;
            leftMotor = -1f / ratio;
        }

        //print(leftMotor + "\t" + rightMotor);
        leftTempPublisher.Loop(leftMotor);
        rightTempPublisher.Loop(rightMotor);

        //sim.leftMotor = leftMotor;
        //sim.rightMotor = rightMotor;
    }

    
}
