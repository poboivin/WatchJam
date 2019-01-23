using UnityEngine;
using System.Collections;

public class Chained : MonoBehaviour 
{
	public GameObject explosion;	
	public GameObject square;// Prefab of explosion effect.
	private float rocketTime = 0f;
	public float TimeAlive = 10f;
	public LifeSpan myOwner;
	private KinematicPlayerControl2 myPlayerControl;



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
			Instantiate(square, transform.position, transform.rotation);
			Destroy(gameObject);
			myPlayerControl = col.GetComponent<KinematicPlayerControl2>();
		//	myPlayerControl.isChained = true;
			myPlayerControl.chainedLocation = transform.position;

			if(col.gameObject != myOwner.gameObject)
			{
				
				if (Camera.main.GetComponent<CamShake>() != null)
					Camera.main.GetComponent<CamShake>().Shake(0.1f, 0.2f);
				
				Destroy(gameObject);
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
