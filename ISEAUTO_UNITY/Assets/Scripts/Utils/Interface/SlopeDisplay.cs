using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlopeDisplay : MonoBehaviour
{
    public Transform vehicle;

    private Text text;
    // Start is called before the first frame update
    void Start() {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        int slope = GetSlope();

        text.text = slope.ToString();
    }

    private int GetSlope() {
        float cos = Vector3.Dot(Physics.gravity, -vehicle.up) / Physics.gravity.magnitude;
        int angle = (int)(Mathf.Acos(cos) * Mathf.Rad2Deg);
        return angle;
    }
}
