using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLineSpawner : MonoBehaviour
{
    public GameObject Dot3D;
    public Vector3 Size;
    [Range(0.001f, 200f)]
    public float Delta;

    private Vector3 direction;
    public float LineLength;
    public float velocity;
   
    public List<GameObject> Emitters;
    public List<List<GameObject>> Starts;
    public List<GameObject> Ends;
    // Start is called before the first frame update
    void Start()
    {
        Ends = new List<GameObject>();
        Starts = new List<List<GameObject>>();
        foreach (var em_tag in FindObjectsOfType<MovingEmitterTag>())
        {
            GameObject em = em_tag.gameObject;
            Emitters.Add(em);
            List<GameObject> start = new List<GameObject>();
            Starts.Add(start);

            foreach (var rec_tag in FindObjectsOfType<MovingReceiverTag>())
            {
                GameObject rec = rec_tag.gameObject;
                Ends.Add(rec);
                Rigidbody r;
                r = rec.GetComponent<Rigidbody>();
                if ((r = rec.GetComponent<Rigidbody>()) == null) 
                 r = rec.AddComponent<Rigidbody>();
                r.isKinematic = true;
                BoxCollider end_col= rec.AddComponent<BoxCollider>();
                end_col.isTrigger = true;
                end_col.size = 0.1f * Vector3.one;
                end_col.center=Vector3.zero;
                
                GameObject st = new GameObject("Start");
                st.transform.parent = em.transform;
                st.transform.localPosition = Vector3.zero;
                MovingLineHandler mlg = em.AddComponent<MovingLineHandler>();
                mlg.start = st.transform;
                mlg.end = rec.transform;
                mlg.Dot3D = Dot3D;
                mlg.Size = Size;
                mlg.Delta = Delta;
                mlg.distance = LineLength;
                mlg.velocity = velocity;

            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
