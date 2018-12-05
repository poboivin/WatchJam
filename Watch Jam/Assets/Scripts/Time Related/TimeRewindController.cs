using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindController : MonoBehaviour
{
    [HideInInspector]
    public TimeBody myTimeBody;
    [HideInInspector]
    public Ammo myAmmo;
    [HideInInspector]
    public PierInputManager myInputManager;
    [HideInInspector]
    public AudioSource myAudioSource;
    public AfterImage myAfterImage;

    public PierInputManager.ButtonName Rewind;
    [SerializeField]
    public bool isRewinding = false;

    private float AmmoTimer = 0;

    public AudioClip rewindEffect;

    private void Awake()
    {
        myTimeBody = GetComponent<TimeBody>();
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
        myAudioSource = GetComponent<AudioSource>();
        myAfterImage = GetComponent<AfterImage>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if( myInputManager.GetAxis( Rewind.ToString() ) > 0.01f )
        {
            myAfterImage.DrawLine();
        }
        if( isRewinding == false && myInputManager.GetAxis( Rewind.ToString() ) > 0.6f 
            && ( myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits ) )
        {
            if( myTimeBody.pointsInTime.Count >= 60 )
                StartRewind();
        }
        if( isRewinding == true && myInputManager.GetAxis( Rewind.ToString() ) < 0.1f )
        {
            StopRewind();
        }

        if( isRewinding )
        {
            AmmoTimer += Time.deltaTime * Settings.s.rewindAmmoFactor;
            if( AmmoTimer >= 1 )
            {
                if( Settings.s.rewindAmmoRegen == true )
                {
                    myAmmo.CurrentAmmo++;

                }
                AmmoTimer = 0;
            }
            if( myAmmo.CurrentAmmo >= myAmmo.MaxAmmo )
            {
                if( isRewinding && Settings.s.noLimits == false )
                {
                    StopRewind();
                }
                myAmmo.CurrentAmmo = myAmmo.MaxAmmo;
            }

            if( myTimeBody.pointsInTime.Count <= 1 )
            {
                StopRewind();
            }
        }
    }

    public void StartRewind()
    {
        myTimeBody.rewind = true;
        isRewinding = true;
        TimeAuraController[] timeAuras = gameObject.GetComponentsInChildren<TimeAuraController>();
        if( timeAuras != null )
        {
            foreach( TimeAuraController aura in timeAuras )
            {
                aura.TurnOnAura( TimeAuraController.Aura.purple );
            }

        }
        if( rewindEffect != null )
        {
            myAudioSource.clip = rewindEffect;
            myAudioSource.Play();
        }
        if( Settings.s.rewindInvincibility == true )
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
        if( timeAuras != null )
        {
            foreach( TimeAuraController aura in timeAuras )
            {
                aura.TurnOffAura();
            }

        }
        if( rewindEffect != null && myAudioSource.clip == rewindEffect )
        {
            StartCoroutine( SoundOffRoutine( myAudioSource, 0.5f ) );
        }
        //player layer
        gameObject.layer = 9;

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
}
