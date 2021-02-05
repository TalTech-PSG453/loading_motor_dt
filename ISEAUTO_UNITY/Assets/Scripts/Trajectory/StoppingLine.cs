using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingLine
{
    private LineRenderer stoppingLine;

    public StoppingLine(LineRenderer lr) {
        stoppingLine = lr;
    }

    public void DrawStoppingLine(double stoppingPosition, Spline spline) {
        float t = Mathf.Clamp01((float)stoppingPosition) * spline.NumSegments;

        var splineValue = spline.Value(t);
        var position = new Vector3(splineValue.x, -splineValue.z, 0);
        var splineDirection = spline.Derivative(t);
        var stoppingLineDirection = Vector3.Cross(splineDirection, Vector3.up).normalized;
        var offset = new Vector3(stoppingLineDirection.x, -stoppingLineDirection.z, 0);
        stoppingLine.SetPosition(0, position + offset);
        stoppingLine.SetPosition(1, position - offset);
    }
}
