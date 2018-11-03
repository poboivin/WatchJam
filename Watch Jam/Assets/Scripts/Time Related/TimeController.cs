using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeController : MonoBehaviour
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
    public AfterImage myAfterImage;

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
    [Header("ammo reload per second")]
    public float stopTimeAmmoFactor = 2f;
    public float passiveAmmoFactor = 2f;
 
    public Vector2 storedMomentum;
    //private bool rt_pressed = false; //right trigger
    public float timeScale;
    public AudioClip rewindEffect;
    public AudioClip stopEffect;

    public UnityEvent OnStartTimeStop;
    public UnityEvent OnStopTimeStop;

    //   public Transform TimeAura; 
    void Awake()
    {
        timeScale = _timeScale;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTimeBody = GetComponent<TimeBody>();
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
        myAudioSource = GetComponent<AudioSource>();
        myAfterImage = GetComponent<AfterImage>();
        originalGravityScale = myRigidbody2D.gravityScale;
        originalMass = myRigidbody2D.mass;
        MatchCounter.Register(this);
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
                StartTimeStop();
            }
                
        }
        if (myInputManager.GetAxis( TimeStop.ToString()) < 0.1f && isStopped == true)
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

        if (myInputManager.GetAxis(Rewind.ToString()) > 0.1f)
        {
            myAfterImage.DrawLine();
        }
        if (myInputManager.GetAxis( Rewind.ToString()) >0.6f && (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits))
        {
            if(isRewinding == false)
                StartRewind();
        }
        if (myInputManager.GetAxis( Rewind.ToString()) <0.1f && isRewinding == true)
        {
            StopRewind();
        }
   
        if (isRewinding)
        {
            AmmoTimer += Time.deltaTime * Settings.s.rewindAmmoFactor;
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
                if (myAmmo.CurrentAmmo >= myAmmo.MaxAmmo)
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
    public void StartRewind()
    {
        myTimeBody.rewind = true;
        isRewinding = true;
        TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        if (timeAuras != null)
        {
            foreach (TimeAuraController aura in timeAuras)
            {
                aura.TurnOnAura(TimeAuraController.Aura.purple);
            }

        }
        if(rewindEffect != null)
        {
            myAudioSource.clip = rewindEffect;
            myAudioSource.Play();
        }
        if(Settings.s.rewindInvincibility == true)
        {
            //rewind layer  
            gameObject.layer = 13;
        }
    }
    public void StopRewind()
    {
        myTimeBody.rewind = false;
        isRewinding = false;
        TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        if (timeAuras != null)
        {
            foreach (TimeAuraController aura in timeAuras)
            {
                aura.TurnOffAura();
            }

        }
        if (rewindEffect != null && myAudioSource.clip == rewindEffect)
        {
            StartCoroutine(SoundOffRoutine(myAudioSource, 0.5f));
        }
        //player layer
        gameObject.layer = 9;

    }
    private IEnumerator SoundOffRoutine(AudioSource s,float time)
    {
        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;

            float perc = timer / time;
        //    Debug.Log(perc);
            s.volume = Mathf.Lerp(1, 0, perc);
            yield return new WaitForEndOfFrame();
        }
        s.Stop();
        s.volume = 1;
        yield return null;
    }
    public void StartTimeStop()
    {
        TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        if(timeAuras != null)
        {
            foreach(TimeAuraController aura in timeAuras)
            {
                aura.TurnOnAura(TimeAuraController.Aura.red);
            }
          
        }
        if (stopEffect != null)
        {
            myAudioSource.clip = stopEffect;
            myAudioSource.Play();
        }
        storedMomentum = Vector2.zero;
        isStopped = true;
        //timeFactor = 0;
        myRigidbody2D.isKinematic = true;
        oldVelocity = myRigidbody2D.velocity;
        myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.angularVelocity = 0;
        myTimeBody.isRecording = false;
        OnStartTimeStop.Invoke();
    }
    public void StopTimeStop()
    {
        TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        if (timeAuras != null)
        {
            foreach (TimeAuraController aura in timeAuras)
            {
                aura.TurnOffAura();
            }

        }
        if (stopEffect != null && myAudioSource.clip == stopEffect)
        {
            StartCoroutine(SoundOffRoutine(myAudioSource, 0.5f));
        }
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
        OnStopTimeStop.Invoke();
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
