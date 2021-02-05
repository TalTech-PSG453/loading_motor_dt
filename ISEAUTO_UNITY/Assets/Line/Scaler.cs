using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public Transform end;
    public Transform start;
    public Transform item;
    public Vector3 original_size;
    public float length;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (item==null)
        {
            return;
        }
        Vector3 ts = 
                item.localScale;
            if (2 * (end.position - item.position).magnitude < length)
            {

                ts.z = 2 * (end.position - item.position).magnitude; /// length;

            }
            else if (2 * (start.position - item.position).magnitude < length)
            {

                ts.z = 2 * (start.position - item.position).magnitude; // / length;

            }
            else
            {
                ts.z = length;

            }

            if (ts.z == 0)
                ts = Vector3.zero;
            else
            {
                ts.x = original_size.x;
                ts.y = original_size.y;
            }

            item.localScale = ts;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ts = item.localScale;
        if (2*(end.position - item.position).magnitude < length)
        {
            
            ts.z = 2 * (end.position - item.position).magnitude; /// length;
            
        }
        else if (2*(start.position - item.position).magnitude < length)
        {
            
            ts.z = 2 * (start.position - item.position).magnitude;// / length;
            
        }
        else
        {
            ts.z = length;
            
        }
        if(ts.z==0)
            ts=Vector3.zero;
        else
        {
            ts.x = original_size.x;
            ts.y = original_size.y;
        }
        item.localScale = ts;
        
        /*print(item.localScale);
        print(2*(end.position - item.position).magnitude);
        print("----------------------- "+gameObject.name);*/
        
        
        /*print(Vector3.Distance(start.position,item.position));
        print(Vector3.Distance(start.position, end.position));
        print(Vector3.Distance(start.position,item.position) > Vector3.Distance(start.position, end.position));*/
    }
}
