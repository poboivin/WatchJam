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

    [SerializeField]
    public bool isRewinding = false;
    [SerializeField]
    public bool isStopped = false;

    
    private float AmmoTimer = 0;
    public float GetAmmoTimer()
    {
        return AmmoTimer;
    }
    [Header("ammo reload per second")]
 
    public Vector2 storedMomentum;
    //private bool rt_pressed = false; //right trigger
    public float timeScale;
    public AudioClip rewindEffect;
    public AudioClip stopEffect;

    public UnityEvent OnStartTimeStop;
    public UnityEvent OnStopTimeStop;

    public PierInputManager.PlayerNumber PlayerId
    {
        get {
            if( myInputManager != null )
                return myInputManager.playerNumber;
            else
                return PierInputManager.PlayerNumber.PC;
        }
    }

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
        
    }

    private void Start()
    {
        MatchCounter.Register( this );
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
        if (isRewinding)
        {
            myAfterImage.DrawLine();

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

            if( myTimeBody.pointsInTime.Count <= 1)
            {
                StopRewind();
            }
        }
        else if (isStopped)
        {
            AmmoTimer += Time.deltaTime * Settings.s.stopTimeAmmoFactor;
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
            SineBob[] timeAuras = gameObject.GetComponentsInChildren<SineBob>();
            if (timeAuras != null)
            {
                foreach (SineBob aura in timeAuras)
                {
                    aura.Speed = 1 /( 1+ storedMomentum.magnitude/200);
                    if(aura.Speed <= 0.03f && Settings.s.stopTimeStoreBullet == true)
                    {
                        StopTimeStop();

                    }
                }

            }
        }
        else
        {
            if(Settings.s.ImmobileRecord == false)
            {
                if(myRigidbody2D.velocity.sqrMagnitude <= 1)
                {
                    myTimeBody.isRecording = false;
                   // Debug.Log("im an not recording");
                }
                else
                {
                  //  Debug.Log("im am recording" + myRigidbody2D.velocity.sqrMagnitude );

                    myTimeBody.isRecording = true;

                }
            }

            if (Settings.s.passiveAmmoRegen == true)
            {
                AmmoTimer += Time.deltaTime * Settings.s.passiveAmmoFactor;
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
       // TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        TimeEffectUIController EffectController = gameObject.GetComponentInChildren<TimeEffectUIController>();
        //if (timeAuras != null)
        //{
        //    foreach (TimeAuraController aura in timeAuras)
        //    {
        //        aura.TurnOnAura(TimeAuraController.Aura.purple);
        //    }

        //}

        if (EffectController != null)
        {
            EffectController.ToggleRewindTime(true);
            
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
        //TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        TimeEffectUIController EffectController = gameObject.GetComponentInChildren<TimeEffectUIController>();
        //if (timeAuras != null)
        //{
        //    foreach (TimeAuraController aura in timeAuras)
        //    {
        //        aura.TurnOffAura();
        //    }

        //}

        if (EffectController != null)
        {
            EffectController.ToggleRewindTime(false);
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
        //TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        TimeEffectUIController EffectController = gameObject.GetComponentInChildren<TimeEffectUIController>();
        if (EffectController != null)
        {
            //foreach(TimeAuraController aura in timeAuras)
            //{
            //    aura.TurnOnAura(TimeAuraController.Aura.red);
            //}

            EffectController.ToggleStoppedTime(true);
          
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
        //TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        TimeEffectUIController EffectController = gameObject.GetComponentInChildren<TimeEffectUIController>();
        if (EffectController != null)
        {
            EffectController.ToggleStoppedTime(false);
            //foreach (TimeAuraController aura in timeAuras)
            //{
            //    aura.TurnOffAura();
            //}

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
            storedMomentum += force ;
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
