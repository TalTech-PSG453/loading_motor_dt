using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerationDisplay : MonoBehaviour
{

    public Rigidbody vehicle;

    public Text total;
    public Text linear;
    public Text angular;
    private Vector3 previousVelocity;

    void FixedUpdate()
    {
        var velocity = vehicle.velocity;
        var acceleration = (velocity - previousVelocity) / Time.deltaTime / Physics.gravity.magnitude;
        previousVelocity = velocity;

        total.text = $"{acceleration.magnitude:F2}g";
        linear.text = $"{Vector3.Dot(acceleration, vehicle.transform.forward):F2}g";
        angular.text = $"{Vector3.Dot(acceleration, vehicle.transform.right):F2}g";
    }
}
