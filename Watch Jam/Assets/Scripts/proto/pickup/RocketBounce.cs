using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class RocketBounce : Rocket
{
    void OnTriggerEnter2D( Collider2D col )
    {
        if( col.tag == "Player" )
        {
            if( col.gameObject != myOwner.gameObject )
            {
                OnExplode();
                if( Camera.main.GetComponent<CamShake>() != null )
                    Camera.main.GetComponent<CamShake>().Shake( 0.1f, 0.2f );
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

                Destroy( gameObject );
            }
        }
        else if( col.gameObject.tag == "destructable" )
        {
            OnExplode();
            if( Camera.main.GetComponent<CamShake>() != null )
                Camera.main.GetComponent<CamShake>().Shake( 0.1f, 0.2f );
            col.gameObject.GetComponent<destructable>().shatter();
            Destroy( gameObject );
        }
        else if( col.tag == "ground" || col.tag == "Obstacle" )
        {
            bool bounced = false;
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            Vector2 inDirection = rigidBody.velocity;
            float maxDistance = Mathf.Abs( col.transform.position.x - transform.position.x ) + Mathf.Abs( col.transform.position.y - transform.position.y );
            RaycastHit2D[] hits = Physics2D.RaycastAll( transform.position, inDirection, maxDistance );

            foreach( var hit in hits )
            {
                if( hit.collider != null && ( hit.collider.tag == "ground" || hit.collider.tag == "Obstacle" ) )
                {
                    Vector2 outDirection = Vector2.Reflect( inDirection, hit.normal );
                    float angle = Vector2.SignedAngle( inDirection, outDirection );
                    rigidBody.velocity = outDirection;

                    Vector3 velocity = outDirection;
                    velocity.Normalize();
                    transform.Rotate( Vector3.forward, angle );

                    // TODO : fix a bug that collides multiple times in the the collision box
                    // the bullets may need to reposition not to collide with the other before bouncing.
                    //Debug.LogFormat( "Contact = {0}, Pos = {6}, Normal = {1}, Input = {2}, Output = {3}, Angle = {4}, Rotation = {5}", hit.collider.name, hit.normal, inDirection, outDirection, angle, transform.rotation, transform.position );

                    bounced = true;
                    break;
                }
            }
            if( bounced == false )
            {
                OnExplode();
                Destroy( gameObject );
            }
        }
        else if( col.tag == "Bullet" )
        {
            Rocket otherRocket = col.GetComponent<Rocket>();
            if( otherRocket != null && otherRocket.myOwner != myOwner )
            {
                OnExplode();
                Destroy( gameObject );
            }
        }

        // Otherwise if the player manages to shoot himself...
        else if( col.gameObject.tag == null )
        {
            OnExplode();
            Destroy( gameObject );
        }
    }
}
