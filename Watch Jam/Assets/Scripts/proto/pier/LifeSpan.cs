using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSpan : MonoBehaviour
{
    TimeController myTimeController;
    TimeRewindController myTimeRewindController;
    TimeStopController myTimeStopController;
    PlayerControl myPlayerControl;
	KinematicPlayerControl2 myPlayerControl2;

    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    Text myText;
    Gun myGun;
	GunCopy2 myGun2;
    public Image lifeDisplay;
    [SerializeField]
    private float currentLife = 0;
    public SpriteRenderer sp;
    public AudioClip clip;
    public AudioSource source;
    //NEW UI Items
    public Image HealthBar;
    public Image HealthShatter;
    bool ShatterActive;
    [HideInInspector]
    public float HurtStartTime;

    bool invincible;
    public float invincibleDuration = 1.5f;
    public void SetInvincibility(bool var)
    { 
        invincible = var;
    }
	// Use this for initialization
	void Start ()
    {
        myAnimator = gameObject.GetComponentInChildren<Animator>();
        currentLife = Settings.s.totalLife;
        myTimeController = gameObject.GetComponent<TimeController>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>();
		myPlayerControl2 = GetComponent<KinematicPlayerControl2>();

        myGun = GetComponentInChildren<Gun>();
		myGun2 = GetComponentInChildren<GunCopy2>();
        myText = GetComponentInChildren<Text>();

        StartCoroutine( ChangeInvincibleCoroutine() );
    }

    IEnumerator ChangeInvincibleCoroutine()
    {
        invincible = true;
        yield return new WaitForSeconds( invincibleDuration );
        invincible = false;
    }

	public void AddLife(float amount)
    {
        currentLife += amount;
        if(currentLife > Settings.s.totalLife)
        {
            currentLife = Settings.s.totalLife;
        }
    }

    public float SubstactLife(float amount)
    {
        if( invincible )
            return 0.0f;
        else
        {
            var decreasedLife = Mathf.Min( currentLife, amount );
            currentLife -= decreasedLife;

            //LEAVING THIS OUT TILL I FIX IT
            //if (HealthShatter != null)
            //{
            //    if (ShatterActive)
            //    {
            //        StopCoroutine(ShowHealthShatter());

            //    }

            //    StartCoroutine(ShowHealthShatter());

            //}


            //if (clip != null && source != null)
            //{
            //    source.clip = clip;
            //    source.Play();
            //}
            //StartCoroutine(ianCoroutine());
            //StopCoroutine(ianCoroutine());


            return decreasedLife;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if(Settings.s.lifeDecay)
            SubstactLife(Time.deltaTime);

        if  (currentLife <= 0)
        {
                Death();
        }
        //lifeDisplay.fillAmount = currentLife / Settings.s.totalLife;
        if (HealthBar != null)
        {
            HealthBar.fillAmount = currentLife / Settings.s.totalLife;
        }
       

        if(myText != null)
        {
            myText.text = (currentLife + "/" + Settings.s.totalLife);
            if(myTimeController.transform.localScale.x < 0  && myText.transform.localScale.x >0)
            {
                myText.transform.localScale = new Vector3(myText.transform.localScale.x *-1, myText.transform.localScale.y, myText.transform.localScale.z);
            }
            if (myTimeController.transform.localScale.x > 0 && myText.transform.localScale.x < 0)
            {
                myText.transform.localScale = new Vector3(myText.transform.localScale.x * -1, myText.transform.localScale.y, myText.transform.localScale.z);
            }
        }
    }
    void Death()
    {
        MatchCounter.Remove(myTimeController);
        myAnimator.enabled = false;
		try {
			myPlayerControl2.enabled = false;
			myGun2.enabled = false;
		}
		catch {
			myPlayerControl.enabled = false;
			myGun.enabled = false;
		}
        myRigidbody2D.freezeRotation = false;
        myTimeController.enabled = false;
        myTimeController.myInputManager.enabled = false;
        if ( myTimeController.isStopped)
        {
            myTimeController.StopTimeStop();
        }
        if ( myTimeController.isRewinding)
        {
            myTimeController.StopRewind();
        }
        
        this.enabled = false;
    }


    public void Hurtzone(float HurtRate)
    {
        if (Time.time - HurtStartTime > HurtRate)
        {
            SubstactLife(1);
            HurtStartTime = Time.time;
        }
    }

    public IEnumerator ianCoroutine()
    {
        if (sp != null)
        {
            sp.material.SetFloat("_FlashAmount", 1);
            yield return new WaitForSeconds(0.2f);

            sp.material.SetFloat("_FlashAmount", 0);
        }

    }

    public IEnumerator ShowHealthShatter()
    {

        ShatterActive = true;
            
        HealthShatter.enabled= true;
        HealthShatter.transform.position = new Vector3(HealthShatter.transform.position.x -.203f, HealthShatter.transform.position.y, HealthShatter.transform.position.z);
        
        yield return new WaitForSeconds(.8f);
        HealthShatter.enabled = false;
        ShatterActive = false;
       

    }


}
