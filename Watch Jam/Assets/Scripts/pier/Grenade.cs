using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosion;            // Prefab of explosion effect.
    public AudioClip boom;					// Audioclip of explosion.

    // Use this for initialization
    void Start ()
    {
		
	}
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }
    void OnTriggerEnter2D(Collider2D col)
    {

   //     Explode();
    }
   public void Explode()
   {

        


        /*
        // Find all the colliders on the Enemies layer within the bombRadius.
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemies"));

        // For each collider...
        foreach (Collider2D en in enemies)
        {
            // Check if it has a rigidbody (since there is only one per enemy, on the parent).
            Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
            if (rb != null && rb.tag == "Enemy")
            {
                // Find the Enemy script and set the enemy's health to zero.
                rb.gameObject.GetComponent<Enemy>().HP = 0;

                // Find a vector from the bomb to the enemy.
                Vector3 deltaPos = rb.transform.position - transform.position;

                // Apply a force in this direction with a magnitude of bombForce.
                Vector3 force = deltaPos.normalized * bombForce;
                rb.AddForce(force);
            }
        }
     */
        // Set the explosion effect's position to the bomb's position and play the particle system.
    //    explosionFX.transform.position = transform.position;
      //  explosionFX.Play();

        if(explosion != null)
        {
             // Instantiate the explosion prefab.
        Instantiate(explosion, transform.position, Quaternion.identity);
        }
       
        if(boom != null)
        {
            // Play the explosion sound effect.
            AudioSource.PlayClipAtPoint(boom, transform.position);

        }

        // Destroy the bomb.
        Destroy(gameObject);
   }
}
