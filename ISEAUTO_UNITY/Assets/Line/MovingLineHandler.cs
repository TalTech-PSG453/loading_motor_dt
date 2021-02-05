using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;


public class MovingLineHandler : MonoBehaviour
{
    // Inspector fields
    
    public GameObject Dot3D;
    public Vector3 Size;
    [Range(0.001f, 200f)]
    public float Delta;

    private Vector3 direction;
    public float distance;
    public float velocity;
    [ReadOnly]
    public float shift=0;
    public Transform start;
    public Transform end;
    public Transform real_end;
    public GameObject real_end_go;
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
        Gizmos.color = Color.red;
        if(real_end!=null)
            Gizmos.DrawLine(start.position, real_end.position);

    }
    
    
    void Update()
    {
        
            
        disp=new List<GameObject>(dots);
        direction = (end.position - start.position).normalized;
        //distance = (end.position - start.position).magnitude;
        if (real_end==null)
        {
            real_end_go = new GameObject("RealEnd");
            real_end = real_end_go.transform;
            real_end.parent = transform;
        }
        real_end.position = 
            start.position
            + direction *
            distance;
        positions.Clear();
        DrawDottedLine(start,real_end);
        disp=new List<GameObject>(dots);
        
        start.rotation = Quaternion.LookRotation(direction, Vector3.Cross(direction, Vector3.up));
        end.rotation = Quaternion.LookRotation(-direction, Vector3.Cross(-direction, Vector3.up));
        if((end.position - start.position).magnitude<distance && !(end.GetComponent<MovingReceiverTag>().triggering))
            Render();
        else
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }

            dots.Clear();
        }
        
        
        
    }

    private void OnEnable()
    {
        if (start != null && end != null)
        {
            


            real_end_go = new GameObject("RealEnd");
            real_end = real_end_go.transform;
            real_end.parent = transform;
            DrawDottedLine(start, end);
        }
        
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


    GameObject GetOneDot3D(string name)
    {
        var dot = Instantiate(Dot3D,start);
        dot.name = name;
        dot.transform.localScale =  Size;
        dot.AddComponent<Scaler>();
        dot.AddComponent<Shifter>();
        Scaler s = dot.GetComponent<Scaler>();
        dot.GetComponent<Shifter>().local=true;
        s.original_size=Size;
        s.end = end;
        s.start = start;
        s.length = Size.z;
        s.item = dot.transform;
        
        return dot;
    }

    public void DrawDottedLine(Transform start, Transform end)
    {
        

        Vector3 point = Vector3.zero;
        direction = (end.position - start.position).normalized;
        float d_rem =
            (real_end.position
             - start.position).magnitude
            % Delta;
        float delta = Delta + d_rem / (((real_end.position - start.position).magnitude - d_rem) / Delta);
        
        
        while (distance > point.z)
        {
            
            positions.Add(point);
            point.z += (delta);
            
            
        }
        positions.Add(point);
        
        
        //Render(Quaternion.LookRotation(direction,Vector3.Cross(direction,Vector3.up+Vector3.left)));
        
    }

    private void Render()
    {
        GameObject dtemp;
        int j = 0;
        if (positions.Count>dots.Count)
        {
            for (int i = (dots.Count>0?dots.Count:0); i < positions.Count; i++)
            {
                dtemp = GetOneDot3D(i.ToString());
                dtemp.transform.localPosition = positions[i];
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
            dtemp.GetComponent<Shifter>().velocity=new Vector3(0,0,velocity);


            if  (distance > (temp.position - start.position).magnitude && 
                (end.position - start.position).magnitude < (temp.position - start.position).magnitude)
            {
                dtemp.GetComponent<MeshRenderer>().enabled = false;
            }
            else if ((real_end.position - start.position).magnitude < (temp.position- start.position).magnitude)
            {

                temp.localPosition= Vector3.zero;
                dots.RemoveAt(i);
                
                dtemp.SetActive(false);
                dots.Add(dtemp);
            }
            else
            {
                dtemp.GetComponent<MeshRenderer>().enabled = true;
            }
            

            if (i == positions.Count-1)
            {
                
                shift = (dots[i - 1].transform.position - end.position).magnitude;
                if (shift >= Delta)
                {
//                    print("Enabling");
                    dtemp.SetActive(true);
                    dtemp.GetComponent<MeshRenderer>().enabled = true;
                    temp.localPosition = Vector3.zero;
                    
                }
                
                
            }
            
            
            
        }

        
        
        

        //positions.Clear();
        
    }

    public void DisableLine()
    {
        
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
