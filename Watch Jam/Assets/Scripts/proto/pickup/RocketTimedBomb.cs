using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTimedBomb : Rocket
{
    public float explosionRangeScale = 10.0f;

    GameObject explosionRangeObject;

    bool isTriggered = false;

    private void Awake()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if( isTriggered )
            return;

        bool enableTrigger = false;
        if (col.tag == "Player")
        {
            if (col.gameObject != myOwner.gameObject)
            {
                enableTrigger = true;
            }
        }
        else if (col.gameObject.tag == "destructable")
        {
            enableTrigger = true;
        }
        else if (col.tag == "ground" || col.tag == "Obstacle")
        {
            enableTrigger = true;
        }
        else if (col.tag == "Bullet")
        {
            Rocket otherRocket = col.GetComponent<Rocket>();
            if (otherRocket != null && otherRocket.myOwner != myOwner)
            {
                enableTrigger = true;
            }
        }

        if(enableTrigger)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            rb.isKinematic = true;
            rocketTime = 0.0f;
            isTriggered = true;
            // may need to show some effect like blinking the area.

        }
    }

    public override void OnExplode()
    {
        base.OnExplode();

        //gameObject.GetComponent<CameraShake>().Shake(0.1f, 0.2f);

        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        Instantiate(explosion, transform.position, randomRotation);

        // Find all the colliders on the Enemies layer within the bombRadius.
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRangeScale, 1 << LayerMask.NameToLayer("Player"));

        // For each collider...
        foreach (Collider2D en in enemies)
        {
            if( en.tag == "Player" && en.gameObject != myOwner.gameObject )
            {
                LifeSpan otherLife = en.gameObject.GetComponent<LifeSpan>();
                if( otherLife )
                {
                    float stealAmount = otherLife.SubstactLife( Settings.s.bulletDamage );
                    if( Settings.s.lifeSteal == true )
                    {
                        myOwner.AddLife( stealAmount );
                    }
                }

            }
        }

        // Destroy the bomb.
        Destroy(gameObject);

    }
}
