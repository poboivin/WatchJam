using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.
    public float rocketTime = 0f;


    void Update()
    {
        // Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
        rocketTime += Time.deltaTime ;
        if (rocketTime >=2f)
        {
            Destroy(gameObject);
            OnExplode();
        }
	}


	void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
        // If it hits an enemy...
        if (col.tag == "Player")
        {
            // ... find the Enemy script and call the Hurt function.
            //col.gameObject.GetComponent<Enemy>().Hurt();

            // Call the explosion instantiation.
            OnExplode();

            // Destroy the rocket.
            Destroy(gameObject);
        }

        else if (col.gameObject.tag == "destructable")
        {
            OnExplode();
            col.gameObject.GetComponent<destructable>().shatter();
            Destroy(gameObject);
        }

        else if (col.tag == "ground" || col.tag == "Obstacle")
        {
            OnExplode();
            Destroy(gameObject);
        }


        // Otherwise if the player manages to shoot himself...
        else if (col.gameObject.tag == null)
        {
            // Instantiate the explosion and destroy the rocket.
            OnExplode();
            Destroy(gameObject);
        }
	}
}
