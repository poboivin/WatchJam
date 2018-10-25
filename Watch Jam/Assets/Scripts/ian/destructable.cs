using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructable : MonoBehaviour {

    public int hp = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shatter()
    {
        //AudioSource.PlayClipAtPoint(crack, transform.position);
        hp--;
        if (hp <= 0)
        {
            //AudioSource.PlayClipAtPoint(smash, transform.position);
            //play shattering animation
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
    }
}
