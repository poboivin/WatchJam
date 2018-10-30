using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSpan : MonoBehaviour
{
    TimeController myTimeController;
    PlayerControl myPlayerControl;
    Rigidbody2D myRigidbody2D;
    Gun myGun;
    public Image lifeDisplay;
    [SerializeField]
    private float currentLife = 0;
  
	// Use this for initialization
	void Start ()
    {
        currentLife = Settings.s.totalLife;
        myTimeController = gameObject.GetComponent<TimeController>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>();
        myGun = GetComponentInChildren<Gun>();
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
      //  Debug.Log(currentLife);
        if(currentLife <= 0)
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
    }
    void Death()
    {
        MatchCounter.Remove(myTimeController);
        myPlayerControl.enabled = false;
        myRigidbody2D.freezeRotation = false;
        myTimeController.enabled = false;
        myGun.enabled = false;
        this.enabled = false;
       // Destroy(this.gameObject);
    }
}
