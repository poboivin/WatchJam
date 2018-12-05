using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Get Time Stop Features from Time Controller
public class TimeStopController : MonoBehaviour
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
    public AudioSource myAudioSource;

    private Vector2 oldVelocity;

    public PierInputManager.ButtonName TimeStop;

    [SerializeField]
    public bool isStopped = false;
    private bool timestoptrigger = false;

    /// <summary>
    /// TODO : Still needs some more refactoring on the ammo stuff
    /// </summary>
    private float AmmoTimer = 0;
    [Header( "ammo reload per second" )]
    public float stopTimeAmmoFactor = 2f;
    public float passiveAmmoFactor = 2f;

    public Vector2 storedMomentum;

    public AudioClip stopEffect;

    public UnityEvent OnStartTimeStop;
    public UnityEvent OnStopTimeStop;

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTimeBody = GetComponent<TimeBody>();
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}

    public void OnDrawGizmos()
    {

        Gizmos.DrawLine( transform.position, transform.position + new Vector3( storedMomentum.x, storedMomentum.y, 0 ).normalized * ( storedMomentum.magnitude / 1000 ) );
        Gizmos.color = Color.red;
        if( isStopped )
        {
            Gizmos.DrawLine( transform.position, transform.position + new Vector3( oldVelocity.x, oldVelocity.y, 0 ).normalized * ( oldVelocity.magnitude / 1000 ) );

        }
    }

    // Update is called once per frame
    void Update()
    {
        if( myInputManager.GetAxis( TimeStop.ToString() ) > 0.5f && timestoptrigger == false && ( myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits ) )
        {
            if( isStopped == false )
            {
                StartTimeStop();
            }
            timestoptrigger = true;
        }
        if( myInputManager.GetAxis( TimeStop.ToString() ) < 0.1f )
        {
            if( isStopped == true )
            {
                StopTimeStop();

            }
            timestoptrigger = false;
        }
        if( isStopped )
        {
            AmmoTimer += Time.deltaTime * stopTimeAmmoFactor;
            if( AmmoTimer >= 1 )
            {
                if( Settings.s.timeStopAmmoRegen == true )
                {
                    myAmmo.CurrentAmmo++;

                }
                AmmoTimer = 0;
            }
            if( myAmmo.CurrentAmmo >= myAmmo.MaxAmmo )
            {
                if( isStopped && Settings.s.noLimits == false )
                {
                    StopTimeStop();
                }
                myAmmo.CurrentAmmo = myAmmo.MaxAmmo;
            }
            SineBob[] timeAuras = gameObject.GetComponentsInChildren<SineBob>();
            if( timeAuras != null )
            {
                foreach( SineBob aura in timeAuras )
                {
                    aura.Speed = 1 / ( 1 + storedMomentum.magnitude / 200 );
                    if( aura.Speed <= 0.03f )
                    {
                        StopTimeStop();

                    }
                }

            }
        }
    }

    public void StartTimeStop()
    {
        TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        if( timeAuras != null )
        {
            foreach( TimeAuraController aura in timeAuras )
            {
                aura.TurnOnAura( TimeAuraController.Aura.red );
            }

        }
        if( stopEffect != null )
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
        if( timeAuras != null )
        {
            foreach( TimeAuraController aura in timeAuras )
            {
                aura.TurnOffAura();
            }

        }
        if( stopEffect != null && myAudioSource.clip == stopEffect )
        {
            StartCoroutine( SoundOffRoutine( myAudioSource, 0.5f ) );
        }
        isStopped = false;
        //timeFactor = 1;
        myRigidbody2D.isKinematic = false;
        if( Settings.s.timeStopKillVelocity == false )
        {
            myRigidbody2D.velocity = oldVelocity;
        }
        myTimeBody.isRecording = true;

        AddForce( storedMomentum );
        storedMomentum = Vector2.zero;
        OnStopTimeStop.Invoke();
    }

    private IEnumerator SoundOffRoutine( AudioSource s, float time )
    {
        float timer = 0;
        while( timer < time )
        {
            timer += Time.deltaTime;

            float perc = timer / time;
            //    Debug.Log(perc);
            s.volume = Mathf.Lerp( 1, 0, perc );
            yield return new WaitForEndOfFrame();
        }
        s.Stop();
        s.volume = 1;
        yield return null;
    }

    public void AddForce( Vector2 force )
    {
        //    force = (force / timeScale);
        // force /= newTimeScale / lastTimeScale; f = m * a
        if( isStopped == true && Settings.s.timeStopStore == true )
        {
            storedMomentum += force * 2;
            //Debug.Log("biding my time");
        }
        else
        {
            // Debug.Log(force);

            myRigidbody2D.AddForce( force, ForceMode2D.Force );
        }


    }

}
