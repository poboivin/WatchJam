using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPiece : MonoBehaviour {

    [SerializeField]
    private GameObject owl;
    public bool isHit = false;
    // check this id not to unshield twice when the bullet hit the border of two shields.
    public int hitId = 0;

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
            hitId = collision.gameObject.GetInstanceID();
            //Debug.Log( string.Format( "{0} hit by {1}", gameObject.GetInstanceID(), hitId ) );
            isHit = true;
            //Destroy( collision.gameObject );
        }
    }

    private void OnTriggerStay2D( Collider2D collision )
    {
        if( collision.gameObject.tag == "Bullet" )
        {
            //if( owl )
            //{
            //    // find a future position
            //    Vector3 bulletVelocity = new Vector3( collision.attachedRigidbody.velocity.x, collision.attachedRigidbody.velocity.y );
            //    Vector3 targetPos = collision.transform.position + bulletVelocity * Time.deltaTime;
            //    Vector3 owlDirection = targetPos - owl.transform.position;
            //    Vector3 newPos = owl.transform.position + owlDirection * Time.deltaTime * 2;
            //    Debug.Log( string.Format( "{0} is pursuing bullet {1}.", owl.transform.position.ToString(), targetPos.ToString() ) );
            //    owl.transform.Translate( owlDirection * Time.deltaTime * 10 );
            //}
        }
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        //if( collision.gameObject.tag == "Bullet" )
        //{
        //    Debug.Log( string.Format( "bullet {0} has gone.", collision.gameObject.GetInstanceID() ) );
        //}
    }

    public void RefreshShield()
    {
        hitId = 0;
        isHit = false;
        gameObject.SetActive( true );
    }
}
