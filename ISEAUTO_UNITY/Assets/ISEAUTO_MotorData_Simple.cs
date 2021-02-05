/*
This message class is generated automatically with 'SimpleMessageGenerator' of ROS#
*/ 

using Newtonsoft.Json;
using RosSharp.RosBridgeClient.Messages.Geometry;
using RosSharp.RosBridgeClient.Messages.Navigation;
using RosSharp.RosBridgeClient.Messages.Sensor;
using RosSharp.RosBridgeClient.Messages.Standard;
using RosSharp.RosBridgeClient.Messages.Actionlib;

namespace RosSharp.RosBridgeClient.Messages
{
public class ISEAUTO_MotorData_Simple : Message
{
[JsonIgnore]
public const string RosMessageName = "iseauto_dt/ISEAUTO_MotorData_Simple";

public float Velocity;
public float Torque;

public ISEAUTO_MotorData_Simple()
{
Velocity = new float();
Torque = new float();
}
}
}

