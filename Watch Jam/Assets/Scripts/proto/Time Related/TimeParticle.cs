using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeParticle : MonoBehaviour, IRewindAble
{
    public ParticleSystem ps;
    public bool isRewinding = false;
   
    public float LifeTime = 0f;
    // Use this for initialization
    void Start () {
		ps = gameObject.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
 
    }
    private void FixedUpdate()
    {
        if (TimeManager._instance.isRewinding)
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
            startRewind();
        LifeTime -= Time.fixedDeltaTime;
        ps.randomSeed = 1;
        ps.Simulate(0, true, true);
        ps.Simulate(LifeTime,true,true);
    }
    private void Record()
    {
        if (isRewinding)
            stopRewind();
        LifeTime += Time.fixedDeltaTime;
  //      ps.randomSeed = 1;
        ps.Simulate(0, true, true);
        ps.Simulate(LifeTime, true, true);
    }
    public void startRewind()
    {
        isRewinding = true;
        
        ps.Pause();
    }
    public void stopRewind()
    {
       
        isRewinding = false;
     //   ps.Simulate(LifeTime, true, true);

     //   ps.Play();
    }
}
