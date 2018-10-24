using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shatter()
    {

        Debug.Log("BAM");
        //AudioSource.PlayClipAtPoint(smash, transform.position);
        Destroy (gameObject);
    }
}
