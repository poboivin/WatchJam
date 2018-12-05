using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewTimeController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D myRigidbody2D;
    [HideInInspector]
    public TimeBody myTimeBody;
    [HideInInspector]
    public Ammo myAmmo;
    [HideInInspector]
    public PierInputManager myInputManager;
    [HideInInspector]
    public AudioSource  myAudioSource;

    private float _timeScale = 1;
    private float newTimeScale = 1;
    private float lastTimeScale = 1;
    private float originalGravityScale;
    private float originalMass;
    private Vector2 oldVelocity;

    public PierInputManager.ButtonName TimeStop;
    public PierInputManager.ButtonName Rewind;
    [SerializeField]
    public bool isRewinding = false;
    [SerializeField]
    public bool isStopped = false;

    
    private float AmmoTimer = 0;
    public float passiveAmmoFactor = 2f;
 
    public Vector2 storedMomentum;
    //private bool rt_pressed = false; //right trigger
    public float timeScale;

    //   public Transform TimeAura; 
    void Awake()
    {
        timeScale = _timeScale;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTimeBody = GetComponent<TimeBody>();
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
        myAudioSource = GetComponent<AudioSource>();
        originalGravityScale = myRigidbody2D.gravityScale;
        originalMass = myRigidbody2D.mass;
        //MatchCounter.Register(this);
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
        if (myInputManager.GetAxis( TimeStop.ToString()) > 0.5f && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits))
        {
            if(isStopped == false)
            {
                isStopped = true;
            }
        }
        if (myInputManager.GetAxis( TimeStop.ToString()) < 0.1f)
        {
            if(isStopped == true)
            {
                isStopped = false;
            }
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

        if (myInputManager.GetAxis( Rewind.ToString()) >0.6f && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits))
        {

            if( isRewinding == false && myTimeBody.pointsInTime.Count >= 60 )
                isRewinding = true;
        }
        if (myInputManager.GetAxis( Rewind.ToString()) <0.1f && isRewinding == true)
        {
            isRewinding = false;
        }

        // rewinding is managed in the TimeRewindController
        if( isRewinding == false && isStopped == false )
        {
            if( Settings.s.passiveAmmoRegen == true )
            {
                AmmoTimer += Time.deltaTime * passiveAmmoFactor;
                if( AmmoTimer >= 1 )
                {
                    myAmmo.CurrentAmmo++;
                    AmmoTimer = 0;
                }
                if( myAmmo.CurrentAmmo >= myAmmo.MaxAmmo )
                {

                    myAmmo.CurrentAmmo = myAmmo.MaxAmmo;
                }
            }
            else
            {
                AmmoTimer = 0;
            }
        }
    }

    public void AddForce(Vector2 force)
    {
    //    force = (force / timeScale);
        // force /= newTimeScale / lastTimeScale; f = m * a
        if(isStopped == true && Settings.s.timeStopStore == true)
        {
            storedMomentum += force *2;
            //Debug.Log("biding my time");
        }
        else
        {
           // Debug.Log(force);

            myRigidbody2D.AddForce(force , ForceMode2D.Force);
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
    public void ResetTimeScale()
    {
        timeScale = 1;
        lastTimeScale = 1;
        _timeScale = 1;
        newTimeScale = 1;
        myRigidbody2D.mass = originalMass;
        myRigidbody2D.gravityScale = originalGravityScale;
    }
    public void SetTimeScale(float scale)
    {
        lastTimeScale = _timeScale;
        _timeScale = Mathf.Abs(scale);
        newTimeScale = _timeScale;
        timeScale = _timeScale;
        //Debug.Log(lastTimeScale + " " + newTimeScale);
    }
    
    void FixedUpdate()
    {
       myRigidbody2D.gravityScale *= newTimeScale / lastTimeScale;
      //  body.velocity *= newTimeScale / lastTimeScale;
       myRigidbody2D.mass /= newTimeScale  / lastTimeScale;
       // body.drag /= newTimeScale / lastTimeScale;
     
        // body.angularVelocity *= newTimeScale / lastTimeScale;
        //vel = body.velocity;
      
        lastTimeScale = newTimeScale;
    }
}
