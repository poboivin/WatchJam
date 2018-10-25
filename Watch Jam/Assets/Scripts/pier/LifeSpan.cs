using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSpan : MonoBehaviour {
    TimeController myTimeController;
    public Image lifeDisplay;
    [SerializeField]
    private float totalLife = 24;
    private float currentLife = 0;
	// Use this for initialization
	void Start () {
        currentLife = totalLife;
        myTimeController = gameObject.GetComponent<TimeController>();
    }
	public void AddLife(float amount)
    {
        currentLife += amount;
    }
    public void SubstactLife(float amount)
    {
        currentLife -= amount;
      //  Debug.Log(currentLife);
        if(currentLife <= 0)
        {
            Death();

        }
    }
	// Update is called once per frame
	void Update ()
    {
        SubstactLife(Time.deltaTime);
        lifeDisplay.fillAmount = currentLife / totalLife;
    }
    void Death()
    {
        MatchCounter.Remove(myTimeController);
        Destroy(this.gameObject);
    }
}
