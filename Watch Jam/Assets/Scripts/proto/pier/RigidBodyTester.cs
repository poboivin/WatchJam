using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidBodyTester : MonoBehaviour {
    public Rigidbody2D body;
    public UnityEvent OnHit;
	// Use this for initialization
	void Start ()
    {
        OnHit.AddListener(stopFunction);

    }
	public void stopFunction()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        OnHit.Invoke();
    }
    // Update is called once per frame
    void FixedUpdate () {
        Vector3 dir =  (transform.position + transform.right * Time.fixedDeltaTime);

        body.MovePosition(new Vector2(dir.x,dir.y) );
	}
}
