using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpyTrack : MonoBehaviour
{
    public GameObject cylinderPrefab;
    public float length;
    public float step;

    // Start is called before the first frame update
    void Start()
    {
        float z = 0;
        while(z < length) {
            var cylinder = Instantiate(cylinderPrefab);
            cylinder.transform.SetParent(transform, true);
            cylinder.transform.localPosition = new Vector3(0f, 0f, z);
            z += step;
        }
    }
}
