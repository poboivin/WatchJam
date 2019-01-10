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
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    Text myText;
    Gun myGun;
    public Image lifeDisplay;
    [SerializeField]
    private float currentLife = 0;
    public SpriteRenderer sp;
    public AudioClip clip;
    public AudioSource source;
  
	// Use this for initialization
	void Start ()
    {
        myAnimator = gameObject.GetComponentInChildren<Animator>();
        currentLife = Settings.s.totalLife;
        myTimeController = gameObject.GetComponent<TimeController>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>();
        myGun = GetComponentInChildren<Gun>();
        myText = GetComponentInChildren<Text>();
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
        currentLife -= amount;
        if (clip != null && source != null)
        {
            source.clip = clip;
            source.Play();
        }
        StartCoroutine(ianCoroutine());
        StopCoroutine(ianCoroutine());



        if (currentLife <= 0)
        {
          
            return currentLife + amount;
        

        }
        else
        {
            return amount;
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
        lifeDisplay.fillAmount = currentLife / Settings.s.totalLife;

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
        myPlayerControl.enabled = false;
        myRigidbody2D.freezeRotation = false;
        myTimeController.enabled = false;
        if ( myTimeController.isStopped)
        {
            myTimeController.StopTimeStop();
        }
        if ( myTimeController.isRewinding)
        {
            myTimeController.StopRewind();
        }
        myGun.enabled = false;
        this.enabled = false;
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
}
