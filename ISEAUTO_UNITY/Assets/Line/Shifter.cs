using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shifter : MonoBehaviour
{
    public Vector3 velocity;

    public bool local = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (local)
        {
            transform.localPosition += velocity * Time.deltaTime;
            return;
        }
        transform.position += velocity * Time.deltaTime;
    }
}
