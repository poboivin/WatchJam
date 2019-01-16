using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainDuration : MonoBehaviour {
	float time = 0;
	public float lifeSpan = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > lifeSpan) {
			Destroy (this.gameObject);
		}
	}
}
