using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour, IRewindAble
{
    public bool rewind = false;
    Rigidbody2D rb;
    public bool isRewinding = false;
    public List<PointInTime> pointsInTime;
    public float RecordTime = 5f;
    public float lifeStart = 0;
    public bool isRecording = true;
	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pointsInTime = new List<PointInTime>();
        lifeStart = TimeManager._instance.curentTime;
    }
	
    // Update is called once per frame
    void Update ()
    {
   
    }
    private void FixedUpdate()
    {
        if (rewind)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    private void Rewind()
    {
        if (!isRewinding)
        {
            startRewind();
        }
        if (pointsInTime.Count > 0)
        {
            PointInTime p = pointsInTime[0];
            transform.position = p.Position;
            transform.rotation = p.Rotation;
            transform.localScale = p.Scale;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            stopRewind();
        }
    }
    private void Record()
    {
        if (isRewinding)
        { 
            stopRewind();
        }
        if (isRecording)
        {
            if (pointsInTime.Count > Mathf.Round(RecordTime / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);

            }
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, transform.localScale));
        }
       
    }
    public void startRewind()
    {
        isRewinding = true ;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.simulated = false;
        }
    }
    public void stopRewind()
    {
        isRewinding = false;
        if (rb != null)
        { 
            rb.isKinematic = false;
            rb.simulated = true;
        }
    }
}
