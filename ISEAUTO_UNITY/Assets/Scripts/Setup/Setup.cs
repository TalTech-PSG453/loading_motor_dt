using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DigitalTwin.ROS;
using DigitalTwin.Trajectory;
using String = DigitalTwin.ROS.std_msgs.String;

public class Setup : MonoBehaviour
{


    public Bridge ROSBridge;
    public TorqueSubscriber torqueSubscriber;
    public DesiredPublisher desiredPublisher;
    public string IP;
    public int Port;


    private void Start()
    {
        ConfigureROSBridge(IP,Port);
        ConfigureControl();
        desiredPublisher.Advertise();
    }

    

        

    

    private void ConfigureROSBridge(string ip, int port)
    {
        ROSBridge.Connect(ip,port);
    }
    private void ConfigureControl()
    {
        torqueSubscriber.Subscribe();
    }

}
