using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moused : MonoBehaviour {
	private TimeController myTimeController;
	private KinematicPlayerControl2 myPlayerControl;


	private float freezeTime = 0f;
	public float freezeDuration = 5.0f;

	// Use this for initialization
	void Awake() {
		myTimeController = gameObject.GetComponent<TimeController> ();
		myPlayerControl = gameObject.GetComponent<KinematicPlayerControl2> ();




		//myPlayerControl.enabled = false;
		Debug.Log ("hi");
	}

	// Update is called once per frame
	void Update () {
		
		freezeTime += Time.deltaTime ;
		if (freezeTime <= freezeDuration) {
			myTimeController.StartTimeStop ();
		} else {

		//	myPlayerControl.enabled = true;
		}
	}
}
