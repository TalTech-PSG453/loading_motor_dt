using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline
{
    private List<Vector3> points;
    private List<Vector3> k0, k1, k2, k3;

    public int NumSegments { get; private set; }

    public int NumPoints => points.Count;

    public Vector3 P0(int segment) {
        return points[segment * 3];
    }

    public Vector3 CP0(int segment) {
        return points[segment * 3 + 1];
    }

    public Vector3 CP1(int segment) {
        return points[segment * 3 + 2];
    }

    public Vector3 P1(int segment) {
        return points[segment * 3 + 3];
    }

    public Spline(List<Vector3> points) {
        this.points = points;
        NumSegments = points.Count < 4 ? 0 : (points.Count - 1) / 3;
        CalculateCoefficients();
    }

    private void CalculateCoefficients() {
        k0 = new List<Vector3>();
        k1 = new List<Vector3>();
        k2 = new List<Vector3>();
        k3 = new List<Vector3>();

        for(int i = 0; i < NumSegments; i++) {
            k0.Add(P0(i));
            k1.Add(3 * (CP0(i) - P0(i)));
            k2.Add(3 * (P0(i) - 2 * CP0(i) + CP1(i)));
            k3.Add(P1(i) + 3 * CP0(i) - 3 * CP1(i) - P0(i));
        }
    }

    private int ParameterToSegment(float t) {
        return Mathf.Clamp((int)t, 0, NumSegments - 1);
    }

    public Vector3 Value(float t) {
        int segment = ParameterToSegment(t);
        t -= (float)segment;

        return k0[segment] + k1[segment] * t + k2[segment] * t * t + k3[segment] * t * t * t;
    }

    public Vector3 Derivative(float t) {
        int segment = ParameterToSegment(t);
        t -= (float)segment;

        return k1[segment] + 2 * k2[segment] * t + 3 * k3[segment] * t * t;
    }

    public static Spline MakeSpline(List<Vector3> nodes) {
        // https://www.particleincell.com/2012/bezier-splines/
        // Note that this page has an error in the next-to-last equation.
        // It should be P_2,i = 2*(K_i+1 - P_1,i+1)
        //
        // Thomas algorithm for solving the system of equations:
        // https://en.wikipedia.org/wiki/Tridiagonal_matrix_algorithm
        List<Vector3> points = new List<Vector3>();

        if(nodes.Count >= 3) {
            int n = nodes.Count - 1;
            float[] b = new float[n];
            Vector3[] d = new Vector3[n], x = new Vector3[n];

            b[0] = 2.0f;
            d[0] = nodes[0] + 2.0f * nodes[1];

            float w;
            for(int i = 1; i < n - 1; i++) {
                w = 1.0f / b[i - 1];
                b[i] = 4.0f - w;
                d[i] = 4.0f * nodes[i] + 2.0f * nodes[i + 1] - w * d[i - 1];
            }

            w = 2.0f / b[n - 2];
            b[n - 1] = 7.0f - w;
            d[n - 1] = 8.0f * nodes[n - 1] + nodes[n] - w * d[n - 2];
            x[n - 1] = d[n - 1] / b[n - 1];
            for(int i = n - 2; i >= 0; i--) {
                x[i] = (d[i] - x[i + 1]) / b[i];
            }

            for(int i = 0; i < n - 1; i++) {
                points.Add(nodes[i]);
                points.Add(x[i]);
                points.Add(2 * nodes[i + 1] - x[i + 1]);
            }
            points.Add(nodes[n - 1]);
            points.Add(x[n - 1]);
            points.Add(0.5f * (nodes[n] + x[n - 1]));
            points.Add(nodes[n]);
        }

        return new Spline(points);
    }

    public string Svg(string color) {
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        s.Append("<path fill=\"none\" stroke=\"" + color + "\" stroke-width=\"0.1\" d=\"");

        if(NumSegments > 0) {
            s.Append("M ");
            s.Append(P0(0).z);
            s.Append(",");
            s.Append(-P0(0).x);
            s.Append(" ");
            s.Append("C ");

            for(int i = 0; i < NumSegments; i++) {
                s.Append(CP0(i).z);
                s.Append(",");
                s.Append(-CP0(i).x);
                s.Append(" ");
                s.Append(CP1(i).z);
                s.Append(",");
                s.Append(-CP1(i).x);
                s.Append(" ");
                s.Append(P1(i).z);
                s.Append(",");
                s.Append(-P1(i).x);
                s.Append(" ");
            }
        }

        s.Append("\" /> ");
        return s.ToString();
    }
}
