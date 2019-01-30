using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindZone : MonoBehaviour {

    [SerializeField] public float rewindingTime = 4.0f;
    [SerializeField] public float duration = 5.0f;

    // Use this for initialization
    void Start () {
        Destroy( this.gameObject, duration );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D( Collider2D col )
    {
        TimeController timeController = col.GetComponent<TimeController>();
        if( timeController != null )
        {
            // just for test, rewind for 5 seconds
            StartCoroutine( RewindCoroutine( timeController, rewindingTime ) );
        }

        //delete if come in to contact with other zone
        if( col.GetComponent<TimeZone>() != null || col.GetComponent<RewindZone>() != null )
        {
            Destroy( this.gameObject );
        }
        //delete if come into contact with teleporter 
        if( col.GetComponent<teleporter>() != null )
        {
            Destroy( this.gameObject );
        }
    }

    private IEnumerator RewindCoroutine( TimeController timeController, float time )
    {
        timeController.StartRewind();
        yield return new WaitForSeconds( time );
        timeController.StopRewind();
    }
}
