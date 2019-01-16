using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPiece : MonoBehaviour {

    public bool isHit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if( collision.gameObject.tag == "Bullet" )
        {
            Destroy( collision.gameObject );
            isHit = true;
        }
    }

    public void RefreshShield()
    {
        isHit = true;
        gameObject.SetActive( true );
    }
}
