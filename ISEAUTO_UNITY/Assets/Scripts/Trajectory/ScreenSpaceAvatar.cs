using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceAvatar : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float maxAngular = 1f;
    public Transform realVehicle;

    public float Speed { get; private set; }

    void Update()
    {
        Speed = Input.GetAxis("Vertical") * maxSpeed;
        float angular = Input.GetAxis("Horizontal") * maxAngular;

        float heading = transform.rotation.eulerAngles.y * Mathf.Deg2Rad + angular * Time.deltaTime;
        float x = transform.position.x + Speed * Mathf.Sin(heading) * Time.deltaTime;
        float z = transform.position.z + Speed * Mathf.Cos(heading) * Time.deltaTime;

        transform.position = new Vector3(x, realVehicle.position.y, z);
        transform.rotation = Quaternion.Euler(0, heading * Mathf.Rad2Deg, 0);
    }
}
