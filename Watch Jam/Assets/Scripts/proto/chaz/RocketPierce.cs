﻿using UnityEngine;
using System.Collections;

public class RocketPierce : MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.
	private float rocketTime = 0f;
	public float TimeAlive = 4f;
	public LifeSpan myOwner;


	private void Start()
	{
		GetComponentInChildren<Animator>().Play("KunaiLoop");
	}

	void Update()
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		rocketTime += Time.deltaTime ;
		if (rocketTime >= TimeAlive)
		{
			Destroy(gameObject);
			OnExplode();
		}
	}


	public void OnExplode()
	{
		//gameObject.GetComponent<CameraShake>().Shake(0.1f, 0.2f);

		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}

	void OnTriggerEnter2D (Collider2D col) 
	{


		if (col.tag == "Player" )
		{
			if(col.gameObject != myOwner.gameObject)
			{
				OnExplode();
				if (Camera.main.GetComponent<CamShake>() != null)
					Camera.main.GetComponent<CamShake>().Shake(0.1f, 0.2f);
				var shooter = myOwner.gameObject.GetComponentInParent<PlayerStatistics>();
				if( shooter != null )
				{
					var casualty = col.gameObject.GetComponentInParent<PlayerStatistics>();
					if( casualty != null )
					{
						shooter.RecordHitTarget( casualty.GetPlayerId() );
					}
					else
					{
						shooter.RecordHitTarget();
					}
				}
			}


		}

		else if (col.gameObject.tag == "destructable")
		{
			OnExplode();
			if (Camera.main.GetComponent<CamShake>() != null)
				Camera.main.GetComponent<CamShake>().Shake(0.1f, 0.2f);
			col.gameObject.GetComponent<destructable>().shatter();
			Destroy(gameObject);
		}

		else if (col.tag == "ground" || col.tag == "Obstacle")
		{
			OnExplode();
			Destroy(gameObject);
		}
		else if (col.tag == "Bullet")
		{
			Rocket otherRocket = col.GetComponent<Rocket>();
			if(otherRocket.myOwner != myOwner)
			{
				OnExplode();
				Destroy(gameObject);
			}

		}

		// Otherwise if the player manages to shoot himself...
		else if (col.gameObject.tag == null)
		{
			OnExplode();
			Destroy(gameObject);
		}
	}
}
