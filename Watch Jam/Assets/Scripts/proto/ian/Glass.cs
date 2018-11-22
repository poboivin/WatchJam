using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shatter()
    {
        //AudioSource.PlayClipAtPoint(smash, transform.position);
        //play shattering animation
        Destroy (gameObject);
    }
}
