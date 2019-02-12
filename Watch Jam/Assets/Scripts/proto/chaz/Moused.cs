using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moused : MonoBehaviour
{
	private TimeController myTimeController;
	private ImouseAble myPlayerControl;
	TimeRewindController myTimeRewindController;
	TimeStopController myTimeStopController;
	LifeSpan myLifeSpan;
    IGun myGun;
	Animator myAnimator;

	private float freezeTime = 0f;
	public float freezeDuration = 5.0f;

	// Use this for initialization
	void Awake()
    {
		myAnimator = gameObject.GetComponentInChildren<Animator>();
		myTimeController = gameObject.GetComponent<TimeController>();
        myPlayerControl = this.gameObject.GetComponent<ImouseAble>();
		myGun = GetComponentInChildren<IGun>();
		myLifeSpan = GetComponent<LifeSpan> ();

	

	
	}

	// Update is called once per frame
	void Update ()
    {
		myAnimator.enabled = false;
		myPlayerControl.setEnable( false);
		myGun.setEnable(false);
		myTimeController.enabled = false;
		myLifeSpan.enabled = false;
		freezeTime += Time.deltaTime ;
		if (freezeTime <= freezeDuration)
        {
			myTimeController.StartTimeStop ();
		}
        else
        {
			UnFreeze ();
		}
	}

	void UnFreeze()
    {
		myTimeController.StopTimeStop ();
		myLifeSpan.enabled = true;
		myLifeSpan.AddLife (2);
		myAnimator.enabled = true;
		myAnimator.enabled = true;
        myPlayerControl.setEnable(true);

        myGun.setEnable(true);
        myTimeController.enabled = true;
		Destroy (this);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if ( freezeTime > 0.1 && col.tag == "Bullet") {
			UnFreeze ();
		}
	}
}
