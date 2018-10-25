using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
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
    public bool isRewinding = false;
    [SerializeField]
    private bool isStopped = false;
    private float AmmoTimer = 0;
    public float rewindAmmoFactor = 4f;
    public float stopTimeAmmoFactor = 2f;
    public float passiveAmmoFactor = 2f;

    private Vector2 storedMomentum;
    private bool rt_pressed = false; //right trigger
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
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void OnDrawGizmos()
    {

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(storedMomentum.x, storedMomentum.y, 0).normalized * (storedMomentum.magnitude /1000));
        Gizmos.color = Color.red;
        if (isStopped)
        {
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(oldVelocity.x, oldVelocity.y, 0).normalized * (oldVelocity.magnitude / 1000));

        }
    }

    bool getTimeStopInput()
    {
        return false;
    }
    void Update()
    {
        if (PierInputManager.GetAxis(inputManager.playerNumber, TimeStop.ToString()) > 0.5f && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits))
        {
            if(isStopped == false)
            {
                StartTimeStop();
            }
                
        }
        if (PierInputManager.GetAxis(inputManager.playerNumber, TimeStop.ToString()) < 0.1f && isStopped == true)
        {
           
                StopTimeStop();

            
        }
        /* button mode 
        if (PierInputManager.GetButtonDown(inputManager.playerNumber, TimeStop) && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits))
        {
            
            
            StartTimeStop();
        }
        if (PierInputManager.GetButtonUp(inputManager.playerNumber, TimeStop) && isStopped == true)
        {
           
            StopTimeStop();
        }*/
        if (PierInputManager.GetAxis(inputManager.playerNumber, Rewind.ToString()) >0.5f && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits))
        {
            if(isRewinding == false)
                StartRewind();
        }
        if (PierInputManager.GetAxis(inputManager.playerNumber, Rewind.ToString()) <0.1f && isRewinding == true)
        {
            StopRewind();
        }
        if (Input.GetButtonDown("Fire2"))
        {
      //      timeScale = factor;
            //body.massh /= factor;
           // body.gravityScale *= factor;
        }
        if (Input.GetButtonUp("Fire2"))
        {
         //   timeScale = 1f;


        }
        if (isRewinding)
        {
            AmmoTimer += Time.deltaTime * rewindAmmoFactor;
            if (AmmoTimer >= 1)
            {
                if(Settings.s.rewindAmmoRegen == true)
                {
                    myAmmo.CurrentAmmo++;

                }
                AmmoTimer = 0;
            }
            if (myAmmo.CurrentAmmo >= myAmmo.MaxAmmo)
            {
                if (isRewinding && Settings.s.noLimits == false)
                {
                    StopRewind();
                }
                myAmmo.CurrentAmmo = myAmmo.MaxAmmo;
            }
        }
        else if (isStopped)
        {
            AmmoTimer += Time.deltaTime * stopTimeAmmoFactor;
            if (AmmoTimer >= 1)
            {
                if (Settings.s.timeStopAmmoRegen == true)
                {
                    myAmmo.CurrentAmmo++;

                }
                AmmoTimer = 0;
            }
            if(myAmmo.CurrentAmmo >= myAmmo.MaxAmmo)
            {
                if(isStopped && Settings.s.noLimits == false)
                {
                    StopTimeStop();
                }
                myAmmo.CurrentAmmo = myAmmo.MaxAmmo;
            }
        }
        else
        {
            if (Settings.s.passiveAmmoRegen == true)
            {
                AmmoTimer += Time.deltaTime * passiveAmmoFactor;
                if (AmmoTimer >= 1)
                {
                    myAmmo.CurrentAmmo++;
                    AmmoTimer = 0;
                }
            }
            else
            {
                AmmoTimer = 0;
            }
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
        storedMomentum = Vector2.zero;
        isStopped = true;
        //timeFactor = 0;
        myRigidbody2D.isKinematic = true;
        oldVelocity = myRigidbody2D.velocity;
        myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.angularVelocity = 0;
        myTimeBody.isRecording = false;
    }
    public void StopTimeStop()
    {
        isStopped = false;
        //timeFactor = 1;
        myRigidbody2D.isKinematic = false;
        if(Settings.s.timeStopKillVelocity == false)
        {
            myRigidbody2D.velocity = oldVelocity;
        }
        myTimeBody.isRecording = true;

        AddForce(storedMomentum);
        storedMomentum = Vector2.zero;
    }
    public void AddForce(Vector2 force)
    {
    //    force = (force / timeScale);
        // force /= newTimeScale / lastTimeScale; f = m * a
        if(isStopped == true && Settings.s.timeStopStore == true)
        {
            storedMomentum += force;
            //Debug.Log("biding my time");
        }
        else
        {
            myRigidbody2D.AddForce(force, ForceMode2D.Force);
        }
     
       
    }
    public void AddImpulse(Vector2 force)
    {
        // if(timeScale!= 1)
        //force = (force /timeScale) /2; 
        force /= timeScale;
       //force /= newTimeScale / lastTimeScale; //f = m * a
        myRigidbody2D.AddForce(force, ForceMode2D.Impulse);
       // Debug.Log("realease");

        // body.velocity += force;

    }
    void FixedUpdate()
    {
       myRigidbody2D.gravityScale *= newTimeScale / lastTimeScale;
      //  body.velocity *= newTimeScale / lastTimeScale;
       myRigidbody2D.mass /= newTimeScale  / lastTimeScale;
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
