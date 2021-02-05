/*
This message class is generated automatically with 'ServiceMessageGenerator' of ROS#
*/ 

using Newtonsoft.Json;
using RosSharp.RosBridgeClient.Messages.Geometry;
using RosSharp.RosBridgeClient.Messages.Navigation;
using RosSharp.RosBridgeClient.Messages.Sensor;
using RosSharp.RosBridgeClient.Messages.Standard;
using RosSharp.RosBridgeClient.Messages.Actionlib;

namespace RosSharp.RosBridgeClient.Services
{
public class RunEncoderRequest : Message
{
[JsonIgnore]
public const string RosMessageName = "iseauto_dt/RunEncoder";

public int Run;

public RunEncoderRequest(int _Run){Run = _Run;
}
}

public class RunEncoderResponse : Message
{
[JsonIgnore]
public const string RosMessageName = "iseauto_dt/RunEncoder";

public String State;
}
}

