﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour {

    public Transform Destination;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D other = collision.GetComponent<Rigidbody2D>();
        if(other != null)
        {
            other.position = Destination.position;
            other.transform.rotation = Destination.rotation;
        }
        
    }
}