using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyTester : MonoBehaviour {
    public Rigidbody2D body;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 dir =  (transform.position + transform.right * Time.fixedDeltaTime);

        body.MovePosition(new Vector2(dir.x,dir.y) );
	}
}
