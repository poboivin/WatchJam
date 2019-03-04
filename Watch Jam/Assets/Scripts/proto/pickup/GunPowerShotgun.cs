using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerShotgun : ISpecialGunPower
{
    public int numSpecialBullets { get; set; }
    public bool enableAbility { get; set; }

    private Gun gun;

    private int numBulletsInShot;   // number of bullets in a single shot
    private float rangeOfAngle;     // shots in range from -angle to +angle

    public GunPowerShotgun( Gun playerGun, int numBoostedBulletCount, int numBulletsInSingleShot, float angle )
    {
        gun = playerGun;
        numSpecialBullets = numBoostedBulletCount;
        numBulletsInShot = numBulletsInSingleShot;
        rangeOfAngle = angle;
        enableAbility = false;
    }

    public void Activate()
    {
        gun.SpecialBar.ToggleRapidFireBar( true );
        enableAbility = true;
    }

    public void Deactivate()
    {
        gun.SpecialBar.ToggleRapidFireBar( false );
        gun.RestoreRocket();
    }

    public void FireBullet( GameObject bullet )
    {
        if( enableAbility )
        {
            if( bullet != null )
            {
                Vector2 velocity = bullet.GetComponent<Rigidbody2D>().velocity;

                if( numBulletsInShot % 2 == 0 )
                {
                    int numNewBulletsInHalf = ( numBulletsInShot ) / 2;
                    
                    for( int i = 1; i <= numNewBulletsInHalf; i++ )
                    {
                        float angle = i * rangeOfAngle / numNewBulletsInHalf;

                        GameObject newBullet1 = UnityEngine.Object.Instantiate( bullet );
                        newBullet1.transform.Rotate( Vector3.forward, -angle );
                        newBullet1.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( -angle );

                        GameObject newBullet2 = UnityEngine.Object.Instantiate( bullet );
                        newBullet2.transform.Rotate( Vector3.forward, angle );
                        newBullet2.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( angle );
                    }
                    UnityEngine.Object.Destroy( bullet );
                }
                else
                {
                    int numNewBulletsInHalf = ( numBulletsInShot - 1 ) / 2;
                    for( int i = 1; i <= numNewBulletsInHalf; i++ )
                    {
                        float angle = i * rangeOfAngle / numNewBulletsInHalf;

                        GameObject newBullet1 = UnityEngine.Object.Instantiate( bullet );
                        newBullet1.transform.Rotate( Vector3.forward, -angle );
                        newBullet1.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( -angle );

                        GameObject newBullet2 = UnityEngine.Object.Instantiate( bullet );
                        newBullet2.transform.Rotate( Vector3.forward, angle );
                        newBullet2.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( angle );
                    }
                }
                //Debug.LogFormat( "Bullet rotation = {0}, velocity = {1}", bullet.transform.rotation, bullet.GetComponent<Rigidbody2D>().velocity.normalized );
            }

            numSpecialBullets--;
            if( numSpecialBullets == 0 )
            {
                enableAbility = false;
            }
        }

    }

    public void Update()
    {
        if( enableAbility )
        {
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }

}

