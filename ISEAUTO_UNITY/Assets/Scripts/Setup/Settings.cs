using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Settings {
    [System.Serializable]
    public struct Trajectory {
        public bool enabled { get; set; }
        public string topic { get; set; }
        public string stateTopic { get; set; }
        public float width { get; set; }

    }

    [System.Serializable]
    public struct IMU {
        public string frame { get; set; }
        public string topic { get; set; }
        public float rotationNoise { get; set; }
        public float accelerationNoise { get; set; }
        public float angularVelocityNoise { get; set; }
        public float frequency { get; set; }
    }

    [System.Serializable]
    public struct GPS {
        public string frame { get; set; }
        public string topic { get; set; }
        public float coordinateNoise { get; set; }
        public float altitudeNoise { get; set; }
        public float frequency { get; set; }
    }

    [System.Serializable]
    public struct LIDAR {
        public string frame { get; set; }
        public int beams { get; set; }
        public int samples { get; set; }
        public float fov { get; set; }
        public float supersampleScale { get; set; }
        public int port { get; set; }
        public float frequency { get; set; }
    }

    [System.Serializable]
    public struct Encoder {
        public bool enabled { get; set; }
        public string type { get; set; }
        public string frame { get; set; }
        public string topic { get; set; }
        public float rpmNoise { get; set; }
        public float torqueNoise { get; set; }
        public float frequency { get; set; }
    }

    [System.Serializable]
    public struct Odometry {
        public bool enabled { get; set; }
        public bool utmEnabled { get; set; }
        public string frame { get; set; }
        public string utmFrame { get; set; }
        public string topic { get; set; }
        public string utmTopic { get; set; }
        public float positionNoise { get; set; }
        public float rotationNoise { get; set; }
        public float velocityNoise { get; set; }
        public float frequency { get; set; }
    }

    [System.Serializable]
    public struct Control {
        public bool cmdVelEnabled { get; set; }
        public string controllerMode { get; set; }
        public string cmdVelTopic { get; set; }
    }

    [System.Serializable]
    public struct Camera {
        public string frame { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public float frameRate { get; set; }
        public string topic { get; set; }
    }

    [System.Serializable]
    public struct DelayedCamera {
        public bool enabled { get; set; }
        public string truePositionVisible { get; set; }
        public float delay { get; set; }
    }

    [System.Serializable]
    public struct RadioBeacons {
        public bool enabled { get; set; }
        public string frame { get; set; }
        public string topic { get; set; }
        public float frequency { get; set; }
        public float distanceNoise { get; set; }
        public bool packetLoss { get; set; }
        public float maxLossProbability { get; set; }
        public float minLossProbability { get; set; }
        public float maxLossDistance { get; set; }
        public float minLossDistance { get; set; }
        public float maxReadingDistance { get; set; }
    }

    [System.Serializable]
    public struct Antenna {
        public bool enabled { get; set; }
        public string frame { get; set; }
        public string topic { get; set; }
        public float noise { get; set; }
        public float rollValue { get; set; }
        public float rollCovariance { get; set; }
        public float frequency { get; set; }
    }

    [System.Serializable]
    public struct Barometer {
        public bool enabled { get; set; }
        public string frame { get; set; }
        public string topic { get; set; }
        public float noise { get; set; }
        public float frequency { get; set; }
    }

    public string world { get; set; }
    public string vehicle { get; set; }
    public string commandTopic { get; set; }
    public string feedbackTopic { get; set; }

    public string frameTopic { get; set; }
    public string baseFrame { get; set; }

    public float gpsOriginLongitude { get; set; }
    public float gpsOriginLatitude { get; set; }
    public float gpsOriginAltitude { get; set; }

    public float friction { get; set; }

    public string ip { get; set; }
    public int port { get; set; }

    public Control control { get; set; }
    public DelayedCamera delayedCamera { get; set; }

    public Trajectory trajectory { get; set; }

    public Encoder encoder { get; set; }
    public Odometry odometry { get; set; }
    public RadioBeacons radioBeacons { get; set; }
    public Antenna antenna { get; set; }
    public Barometer barometer { get; set; }

    public IMU[] imu { get; set; }
    public GPS[] gps { get; set; }
    public LIDAR[] lidar { get; set; }
    public Camera[] camera { get; set; }
}
