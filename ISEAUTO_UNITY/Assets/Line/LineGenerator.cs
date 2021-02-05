using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;


public class LineGenerator : MonoBehaviour
{
    // Inspector fields
    
    public GameObject Dot3D;
    public Vector3 Size;
    [Range(0.001f, 200f)]
    public float Delta;

    private Vector3 direction;
    public float velocity;
    [ReadOnly]
    public float shift=0;
    public Transform start;
    public Transform end;

    private Vector3 startS;
    private Vector3 startE;
    //Utility fields
    public List<Vector3> positions = new List<Vector3>();
    public List<GameObject> dots = new List<GameObject>();

    public List<GameObject> disp;
    // Update is called once per frame
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawLine(start.position, end.position);
        

    }
    
    
    void Update()
    {
        
        if (startS != start.position || startE != end.position)
        {
            positions.Clear();
            dots.Clear();
            DrawDottedLine(start,end);
            startS = start.position;
            startE = end.position;
        }
            
        disp=new List<GameObject>(dots);

       
        
        disp=new List<GameObject>(dots);
        start.rotation = Quaternion.LookRotation(direction, Vector3.Cross(direction, Vector3.up));
        Render(Quaternion.LookRotation(direction,Vector3.Cross(direction,Vector3.up)));
        
        
    }

    private void OnEnable()
    {
        startS = start.position;
        startE = end.position;
        DrawDottedLine(start,end);
    }

    private void OnDisable()
    {
        positions.Clear();
        foreach (var dot in dots)
        {
            Destroy(dot);
            
        }
        dots.Clear();
    }


    GameObject GetOneDot3D(Quaternion orientation, string name)
    {
        var dot = Instantiate(Dot3D,transform);
        dot.name = name;
        dot.transform.localScale =  Size;
        dot.AddComponent<Scaler>();
        dot.AddComponent<Shifter>();
        Scaler s = dot.GetComponent<Scaler>();
        s.original_size=Size;
        s.end = end;
        s.start = start;
        s.length = Size.z;
        s.item = dot.transform;
        dot.transform.rotation = orientation;
        
        return dot;
    }

    public void DrawDottedLine(Transform start, Transform end)
    {

        Vector3 point = start.position;
        direction = (end.position - start.position).normalized;
        float d_rem = (end.position - start.position).magnitude % Delta;
        Delta += d_rem / (((end.position - start.position).magnitude - d_rem) / Delta);
        
        
        while ((end.position - start.position).magnitude > (point - start.position).magnitude)
        {
            
            positions.Add(point);
            point += (direction * (Delta));
            
            
        }
        positions.Add(point);
        
        
        //Render(Quaternion.LookRotation(direction,Vector3.Cross(direction,Vector3.up+Vector3.left)));
        
    }

    private void Render(Quaternion orientation)
    {
        GameObject dtemp;
        int j = 0;
        if (positions.Count>dots.Count)
        {
            for (int i = (dots.Count>0?dots.Count:0); i < positions.Count; i++)
            {
                dtemp = GetOneDot3D(orientation,i.ToString());
                dtemp.transform.position = positions[i];
                dtemp.transform.rotation = orientation;
                dots.Insert(0,dtemp);
            }


        }
        
        else if (positions.Count<dots.Count)
        {
            for (int i = (positions.Count>0?positions.Count-1:0); i < dots.Count; i++)
            {
                var t = dots[i];
                Destroy(t);
                dots.RemoveAt(i);
                
                
            }
        }
        
        //print("Dots: " + dots.Count+"\n"+"Positions: "+positions.Count);

        for (int i =0 ; i <positions.Count ; i++)
        {
            
            dtemp = dots[i];
            
            Transform temp = dtemp.transform;
            dtemp.GetComponent<Shifter>().velocity=velocity*direction;

            
            temp.rotation = orientation;
            
            if ((end.position - start.position).magnitude < (temp.position- start.position).magnitude)
            {

                temp.position= start.position;
                dots.RemoveAt(i);
                
                dtemp.SetActive(false);
                dots.Add(dtemp);
            }else if ((end.position - start.position).magnitude < (temp.position- end.position).magnitude)
            {

                temp.position= end.position; 
                
                dots.RemoveAt(i);
                
                dtemp.SetActive(false);
                dots.Insert(0,dtemp);
            }

            if (i == positions.Count-1)
            {
                
                shift = (dots[i - 1].transform.position - end.position).magnitude;
                if (shift >= Delta)
                {
                   // print("Enabling");
                    dtemp.SetActive(true);
                    temp.position = start.position;
                    
                }
                
                
            }
            
            
            
        }

        
        
        

        //positions.Clear();
        
    }
    

    bool Vector3Compare_AnyGreater(Vector3 a, Vector3 b)
    {
        for (int i = 0; i < 3;i++)
        {
            if (a[i] > b[i])
                return true;
        }

        return false;
    }
}
