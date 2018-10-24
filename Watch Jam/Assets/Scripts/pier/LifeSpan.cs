using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSpan : MonoBehaviour {
    public Image lifeDisplay;
    [SerializeField]
    private float totalLife = 24;
    private float currentLife = 0;
	// Use this for initialization
	void Start () {
        currentLife = totalLife;

    }
	public void AddLife(float amount)
    {
        currentLife += amount;
    }
    public void SubstactLife(float amount)
    {
        currentLife -= amount;
        Debug.Log(currentLife);
        if(currentLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }
	// Update is called once per frame
	void Update () {
        currentLife -= Time.deltaTime;
        lifeDisplay.fillAmount = currentLife / totalLife;
        if (currentLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
