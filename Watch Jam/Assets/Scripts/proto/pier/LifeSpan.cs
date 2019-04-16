using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LifeSpan : MonoBehaviour
{
    TimeController myTimeController;
    TimeRewindController myTimeRewindController;
    TimeStopController myTimeStopController;
    PlayerControl myPlayerControl;
	KinematicPlayerControl2 myPlayerControl2;

    Rigidbody2D myRigidbody2D;
    [SerializeField]
    Animator myAnimator;
    Text myText;
    Gun myGun;
	GunCopy2 myGun2;
    public Image lifeDisplay;
    [SerializeField]
    private float currentLife = 0;
    public SpriteRenderer[] sp;
    public AudioClip clip;
    public AudioSource source;
    public AudioClip Impact;
    //NEW UI Items
    public AboveHeadHealthbar HealthBar;
    public Image HealthShatter;
    bool ShatterActive;
    [HideInInspector]
    public float HurtStartTime;
    [HideInInspector]
    public UnityEvent OnLifeUpdate;
    [SerializeField]
    bool invincible;
    public float invincibleDuration = 1.0f;


    public void SetInvincibility(bool var)
    { 
       // invincible = var;
    }
	// Use this for initialization
	void Start ()
    {
        myAnimator = gameObject.GetComponentInChildren<Animator>();
        SetLife(Settings.s.totalLife);
       
        myTimeController = gameObject.GetComponent<TimeController>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>();
		myPlayerControl2 = GetComponent<KinematicPlayerControl2>();

        myGun = GetComponentInChildren<Gun>();
		myGun2 = GetComponentInChildren<GunCopy2>();
        myText = GetComponentInChildren<Text>();

        sp = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine( ChangeInvincibleCoroutine(invincibleDuration) );

        
    }

    

    private void SetLife(float amount)
    {
        currentLife = amount;
        OnLifeUpdate.Invoke();
    }

    public float GetLife()
    {
        return currentLife;
    }

	public void AddLife(float amount)
    {
        
        SetLife(currentLife + amount);

        if (currentLife > Settings.s.totalLife)
        {
           // currentLife = Settings.s.totalLife;
            SetLife(Settings.s.totalLife);
        }
    }

    public float SubstactLife(float amount)
    {
        if( invincible)
        {
            Debug.LogWarning("not hurt");

            return 0.0f;
        }
           
        else
        {
            var decreasedLife = Mathf.Min( currentLife, amount );
            SetLife(currentLife - decreasedLife);
            myAnimator.SetTrigger("HurtTrigger");
            Debug.Log("Hurt Trigger");
            HealthBar.UpdateUI(currentLife);
            source.clip = Impact;
            source.Play();
            

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


            StartCoroutine(ChangeInvincibleCoroutine(invincibleDuration));
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
        //if (HealthBar != null)
        //{
        //    HealthBar.fillAmount = currentLife / Settings.s.totalLife;
        //}
       

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


    public IEnumerator ShowHealthShatter()
    {

        ShatterActive = true;
            
        HealthShatter.enabled= true;
        HealthShatter.transform.position = new Vector3(HealthShatter.transform.position.x -.203f, HealthShatter.transform.position.y, HealthShatter.transform.position.z);
        
        yield return new WaitForSeconds(.8f);
        HealthShatter.enabled = false;
        ShatterActive = false;
       

    }

    IEnumerator ChangeInvincibleCoroutine(float invulDur)
    {
        
        invincible = true;
        if (sp != null)
        {
            

            for (int i = 0; i < (invulDur/0.2f); i++)
            {
                //Debug.Log(invincible);
                foreach (SpriteRenderer spr in sp)
                {
                    spr.material.SetColor("_Tint", new Color(0.4f, 0.4f, 0.4f, 1.0f));
                }
                yield return new WaitForSeconds(0.1f);
                foreach (SpriteRenderer spr in sp)
                {
                    spr.material.SetColor("_Tint", new Color(0.0f, 0.0f, 0.0f, 0.0f));
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        invincible = false;
    }


}
