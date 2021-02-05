using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LineProto : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    public GameObject DashBase;
    
    
    private GameObject backup;
    
    [Range(0,100)]
    public float length;
    [Range(0,100)]
    public float delta;
    
    
    public float offset;
    public float L;

    public Vector3 direction;
    public Quaternion orientation;
    public float distance;
    public float slide;

    public List<Vector3> points;
    public List<GameObject> dashes;

    // Start is called before the first frame update
    void Start()
    {
        updateConstants();
        points=generatePoints();
        backup = getDash(StartPoint.position-direction*L,orientation);
        backup.name = "Backup";
        backup.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 s = StartPoint.position;
        Vector3 e = EndPoint.position;
        points=generatePoints();
        //updateDash(backup.transform,StartPoint.position-direction*L,orientation);
        updateLine();
        
        /*GameObject dash = backup;
        scaleDashEnd(dash.transform,EndPoint);
        scaleDashStart(dash.transform,StartPoint);*/
        
    }

    List<Vector3> generatePoints()
    {
        points.Clear();
        updateConstants();
        List<Vector3> ret=new List<Vector3>();
        Vector3 s = StartPoint.position;
        Vector3 e = EndPoint.position;
        Vector3 v = s;
        while ((v-s).magnitude<distance)
        {
            ret.Add(v);
            
            v += direction * L;
        }



        return ret;

    }
    
    void updateDash(Transform dashBase, Vector3 position, Quaternion rotation)
    {
        
        
        dashBase.position = position;
        dashBase.rotation = rotation;
        
        
        
        GameObject dash = dashBase.GetChild(0).gameObject;
        Vector3 scale = dash.transform.localScale;
        scale.z = length;
        dash.transform.localScale = scale;
        Vector3 pos = dash.transform.localPosition;
        pos.z = offset;
        dash.transform.localPosition = pos;
        
    }

    void resetBackup(Transform dashBase)
    {
        updateDash(dashBase, StartPoint.position - direction * L, orientation);
    }
    GameObject getDash(Vector3 position, Quaternion rotation)
    {
        GameObject ret = Instantiate(DashBase);
        ret.transform.SetParent(transform);
        
        ret.transform.position = position;
        ret.transform.rotation = rotation;
        
        
        
        GameObject dash = ret.transform.GetChild(0).gameObject;
        Vector3 scale = dash.transform.localScale;
        scale.z = length;
        dash.transform.localScale = scale;
        Vector3 pos = dash.transform.localPosition;
        pos.z = offset;
        dash.transform.localPosition = pos;

        return ret;
    }

    
    
    void updateConstants()
    {
        L = delta + length;
        offset = delta + length / 2;
        direction = EndPoint.position - StartPoint.position;
        direction = direction.normalized;
        orientation = Quaternion.LookRotation(direction, Vector3.Cross(direction, transform.up));
        distance = (EndPoint.position - StartPoint.position).magnitude;
    }

    bool scaleDashEnd(Transform dashBase, Transform end)
    {
        GameObject dash = dashBase.GetChild(0).gameObject;
        bool ret = false;
        Vector3 scale = dash.transform.localScale;
        Vector3 pos = dash.transform.localPosition;
        Vector3 endDelta = end.position - dashBase.position;
        float endDeltaLength = endDelta.magnitude;
        
        if (endDeltaLength < L)
        {
            float lengthScaled = endDeltaLength - delta;
            pos.z = delta + lengthScaled / 2;
            scale.z = lengthScaled;
            ret = true;
        }

        if (endDeltaLength < delta)
        {
            scale.z = 0;
        }

        

        dash.transform.localScale = scale;
        dash.transform.localPosition = pos;

        return ret;

    }
    void scaleDashStart(Transform dashBase, Transform start)
    {
        GameObject dash = dashBase.GetChild(0).gameObject;
        
        Vector3 scale = dash.transform.localScale;
        Vector3 pos = dash.transform.localPosition;
        Vector3 startDelta = start.position - dashBase.position;
        float startDeltaLength = startDelta.magnitude;
        if (startDeltaLength > L)
            return;
        if (startDeltaLength<=delta||dashBase.localPosition.z<0)
        {
            pos.z = offset;
            scale.z = length;
            
        }else if (startDeltaLength < L)
        {
            float lengthScaled = L-startDeltaLength;
            pos.z = L-lengthScaled/2;
            scale.z = lengthScaled;

        }else if (dashBase.localPosition.z > 0)
        {
            
            scale.z = 0;
        }
        
        

        

        dash.transform.localScale = scale;
        dash.transform.localPosition = pos;



    }
    
    
    
    
    
    
    void updateLine()
    {
        int sizeP = points.Count;
        int sizeD = dashes.Count;
        Vector3 s = StartPoint.position;
        Vector3 e = EndPoint.position;
        Vector3 shift = direction * slide;
        if (sizeP > sizeD)
        {
            for (int i = (sizeD < 1 ? 0 : sizeD - 1); points.Count != dashes.Count; i++)
            {
                GameObject g = getDash(points[i]+shift, orientation);
                g.name = i.ToString();
                dashes.Add(g);
                
            }
            sizeD = dashes.Count;
        }else if (sizeP < sizeD)
        {
            for (int i = (sizeP < 1 ? 0 : sizeP - 1); points.Count != dashes.Count; i++)
            {
                GameObject g =  dashes[i];
                dashes.RemoveAt(i);
                Destroy(g);
            }
            sizeD = dashes.Count;
        }
    
        for (int i  = 0;i<sizeD;i++)
        {
            GameObject dash = dashes[i];
            updateDash(dash.transform,points[i]+shift,orientation);
            if (scaleDashEnd(dash.transform, EndPoint))
            {
                if (backup != null)
                {
                    backup.SetActive(true);
                    updateDash(backup.transform, dashes[0].transform.position + shift, orientation);
                    scaleDashStart(backup.transform, StartPoint);
                    dashes.Insert(0, backup);
                    print("delete backup");
                    backup = null;
                }


            }

            if ((dash.transform.position - StartPoint.position).magnitude > distance)
            {
                print("Set backup");
                backup = dash;
                dashes.RemoveAt(i);
                resetBackup(backup.transform);
                backup.SetActive(false);
            }
            
            
            
        }










    }
    
    
    
}
