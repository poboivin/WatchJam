using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {
    public Rigidbody2D body;
    public TimeBody myTimeBody;
    public Vector2 oldVelocity;
    public Ammo myAmmo;
    bool first = true;

    [SerializeField]
    private float _timeScale = 1;
    private float newTimeScale = 1;
    private float lastTimeScale = 1;
    public Vector2 vel;
    public float factor = 0.25f;
    public PierInputManager inputManager;
    public PierInputManager.ButtonName TimeStop;
    public PierInputManager.ButtonName Rewind;
    [SerializeField]
    private bool isRewinding = false;
    [SerializeField]
    private bool isStopped = false;
    private float AmmoTimer = 0;
    public float rewindAmmoFactor = 4f;
    public float stopTimeAmmoFactor = 2f;
    public bool noLimits;
    public float timeScale
    {
        get { return _timeScale; }
        set
        {
            lastTimeScale = _timeScale;

            _timeScale = Mathf.Abs(value);
            newTimeScale = _timeScale;
        }
    }

    void Awake()
    {
        timeScale = _timeScale;

    }

    void Update()
    {
        if (PierInputManager.GetButtonDown(inputManager.playerNumber, TimeStop) && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || noLimits))
        {
            
            
            StartTimeStop();
        }
        if (PierInputManager.GetButtonUp(inputManager.playerNumber, TimeStop) && isStopped == true)
        {
           
            StopTimeStop();
        }
        if (PierInputManager.GetButtonDown(inputManager.playerNumber, Rewind) && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || noLimits))
        {
            StartRewind();
        }
        if (PierInputManager.GetButtonUp(inputManager.playerNumber, Rewind)&& isRewinding == true)
        {
            StopRewind();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            timeScale = factor;
            //body.massh /= factor;
           // body.gravityScale *= factor;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            timeScale = 1f;


        }
        if (isRewinding)
        {
            AmmoTimer += Time.deltaTime * rewindAmmoFactor;
            if (AmmoTimer >= 1)
            {
                myAmmo.CurrentAmmo++;
                AmmoTimer = 0;
            }
            if (myAmmo.CurrentAmmo == myAmmo.MaxAmmo)
            {
                if (isRewinding && noLimits == false)
                {
                    StopRewind();
                }

            }
        }
        else if (isStopped)
        {
            AmmoTimer += Time.deltaTime * stopTimeAmmoFactor;
            if (AmmoTimer >= 1)
            {
                myAmmo.CurrentAmmo++;
                AmmoTimer = 0;
            }
            if(myAmmo.CurrentAmmo == myAmmo.MaxAmmo)
            {
                if(isStopped && noLimits == false)
                {
                    StopTimeStop();
                }

            }
        }
        else
        {
            AmmoTimer = 0;
        }
       
    }
    public void StartRewind()
    {
        myTimeBody.rewind = true;
        isRewinding = true;

    }
    public void StopRewind()
    {
        myTimeBody.rewind = false;
        isRewinding = false;
    }
    public void StartTimeStop()
    {
        isStopped = true;
        //timeFactor = 0;
        body.isKinematic = true;
        oldVelocity = body.velocity;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        myTimeBody.isRecording = false;
    }
    public void StopTimeStop()
    {
        isStopped = false;
        //timeFactor = 1;
        body.isKinematic = false;
        body.velocity = oldVelocity;
        myTimeBody.isRecording = true;
    }
    public void AddForce(Vector2 force)
    {
    //    force = (force / timeScale);
        // force /= newTimeScale / lastTimeScale; f = m * a
        body.AddForce(force, ForceMode2D.Force);
    }
    public void AddImpulse(Vector2 force)
    {
        // if(timeScale!= 1)
        //force = (force /timeScale) /2; 
        force /= timeScale;
       //force /= newTimeScale / lastTimeScale; //f = m * a
        body.AddForce(force, ForceMode2D.Impulse);

       // body.velocity += force;
        
    }
    void FixedUpdate()
    {
       body.gravityScale *= newTimeScale / lastTimeScale;
      //  body.velocity *= newTimeScale / lastTimeScale;
       body.mass /= newTimeScale  / lastTimeScale;
       // body.drag /= newTimeScale / lastTimeScale;
     
        // body.angularVelocity *= newTimeScale / lastTimeScale;
        //vel = body.velocity;
        if(vel.magnitude> 30)
        {
            Debug.Log(newTimeScale + " " +lastTimeScale);
        }
        lastTimeScale = newTimeScale;
    }
}
