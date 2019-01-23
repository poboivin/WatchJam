using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainDuration : MonoBehaviour {
	float time = 0;
	public float lifeSpan = 5.0f;
	private Chained2 d;

	void Awake(){
		d = GetComponent<Chained2> ();
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireSphere (gameObject.transform.position, 5);
	}
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > lifeSpan) {
			Destroy (this.gameObject);
		}
	}
}
