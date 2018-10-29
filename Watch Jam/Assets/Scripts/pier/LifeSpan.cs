using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSpan : MonoBehaviour
{
    TimeController myTimeController;
    PlayerControl myPlayerControl;
    Rigidbody2D myRigidbody2D;
    public Image lifeDisplay;
    [SerializeField]
    private float totalLife = 24;
    private float currentLife = 0;
  
	// Use this for initialization
	void Start () {
        currentLife = totalLife;
        myTimeController = gameObject.GetComponent<TimeController>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>();
    }
	public void AddLife(float amount)
    {
        currentLife += amount;
        if(currentLife > totalLife)
        {
            currentLife = totalLife;


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
        lifeDisplay.fillAmount = currentLife / totalLife;
    }
    void Death()
    {
        MatchCounter.Remove(myTimeController);
        myPlayerControl.enabled = false;
        myRigidbody2D.freezeRotation = false;
        myTimeController.enabled = false;
        this.enabled = false;
       // Destroy(this.gameObject);
    }
}
