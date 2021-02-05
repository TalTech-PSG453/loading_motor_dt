using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingReceiverTag : MonoBehaviour
{
    public bool triggering;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        triggering = true;
    }

    private void OnTriggerExit(Collider other)
    {
        triggering = false;
    }
}
