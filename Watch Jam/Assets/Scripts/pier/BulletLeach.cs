using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLeach : MonoBehaviour {
    public LifeSpan Owner;
    public float amount = 4f;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        LifeSpan otherLife = other.gameObject.GetComponent<LifeSpan>();
        if(otherLife != null)
        {
       //     Debug.Log("hey");
            Owner.AddLife(amount);
            otherLife.SubstactLife(amount);
        }
    }
}
