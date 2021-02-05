using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawtooth : MonoBehaviour
{
    public GameObject sawtoothPrefab;
    public float length;

    // Start is called before the first frame update
    void Start()
    {
        float z = 0;
        while(z < length) {
            var sawtooth = Instantiate(sawtoothPrefab);
            sawtooth.transform.SetParent(transform, false);
            sawtooth.transform.localPosition = new Vector3(0f, 0f, z);
            z += 0.18f;
        }
    }
}
