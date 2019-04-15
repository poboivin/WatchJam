using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerShotgun : GunPowerGeneric
{
    private int numBulletsInShot;   // number of bullets in a single shot
    private float rangeOfAngle;     // shots in range from -angle to +angle
    private GameStatisticsManager statisticsManager;

    public GunPowerShotgun( Gun playerGun, PickupShotgun pickupInfo )
        : base( playerGun, pickupInfo.numTotalShots )
    {
        numBulletsInShot = pickupInfo.numOfBulletsInOneShot;
        rangeOfAngle = pickupInfo.angle;

        damageMultiplier = pickupInfo.damageMultiplier;
        statisticsManager = UnityEngine.Object.FindObjectOfType<GameStatisticsManager>();
    }

    public override void Activate()
    {
        base.Activate();
        gun.SpecialBar.ToggleRapidFireBar( true );
        gun.SpecialBar.ChangeBarColor(2);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gun.SpecialBar.ToggleRapidFireBar( false );
    }

    public override void FireBullet( GameObject bullet )
    {
        base.FireBullet( bullet );
        if( enableAbility )
        {
            if( bullet != null )
            {
                Vector2 velocity = bullet.GetComponent<Rigidbody2D>().velocity;
                
                Vector3 rotationAxis = Vector3.forward;
                Vector2 velDirection = velocity.normalized;
                if( velDirection == Vector2.left )
                    rotationAxis = Vector3.back;

                //Debug.LogFormat( "Bullet vel = {0}, unit vel = {1}, rotation axis = {2}", velocity, velDirection, rotationAxis );

                if( numBulletsInShot % 2 == 0 )
                {
                    float angleInArc = ( 2 * rangeOfAngle ) / ( numBulletsInShot - 1 );

                    int numNewBulletsInOneSide = numBulletsInShot / 2 - 1;
                    for( int i = 1; i <= numNewBulletsInOneSide; i++ )
                    {
                        float angle = angleInArc / 2 + i * angleInArc;

                        GameObject newBullet1 = UnityEngine.Object.Instantiate( bullet );
                        newBullet1.transform.Rotate( rotationAxis, -angle );
                        newBullet1.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( -angle );
                        newBullet1.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;

                        GameObject newBullet2 = UnityEngine.Object.Instantiate( bullet );
                        newBullet2.transform.Rotate( rotationAxis, angle );
                        newBullet2.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( angle );
                        newBullet2.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;
                    }

                    // rotate original bullet in half angle upward, and create new one in the opposite half angle
                    GameObject newBullet = UnityEngine.Object.Instantiate( bullet );
                    newBullet.transform.Rotate( rotationAxis, -angleInArc / 2 );
                    newBullet.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( -angleInArc / 2 );
                    newBullet.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;

                    bullet.transform.Rotate( rotationAxis, angleInArc / 2 );
                    bullet.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( angleInArc / 2 );
                    bullet.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;
                }
                else
                {
                    int numNewBulletsInOneSide = ( numBulletsInShot - 1 ) / 2;
                    for( int i = 1; i <= numNewBulletsInOneSide; i++ )
                    {
                        float angle = i * rangeOfAngle / numNewBulletsInOneSide;

                        GameObject newBullet1 = UnityEngine.Object.Instantiate( bullet );
                        newBullet1.transform.Rotate( rotationAxis, -angle );
                        newBullet1.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( -angle );
                        newBullet1.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;


                        GameObject newBullet2 = UnityEngine.Object.Instantiate( bullet );
                        newBullet2.transform.Rotate( rotationAxis, angle );
                        newBullet2.GetComponent<Rigidbody2D>().velocity = velocity.Rotate( angle );
                        newBullet2.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;
                    }
                    bullet.GetComponent<BulletLeach>().damageMultiplier = damageMultiplier;
                }
                //Debug.LogFormat( "Bullet rotation = {0}, velocity = {1}", bullet.transform.rotation, bullet.GetComponent<Rigidbody2D>().velocity.normalized );

                if( statisticsManager != null )
                {
                    // The reason why numBulletsInShot - 1 is Gun script also records a shot in Update function.
                    statisticsManager.RecordFire( (int)gun.GetPlayerId(), numBulletsInShot - 1 );
                }
            }
        }
    }

    public override void Update()
    {
        if( enableAbility )
        {
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }

}

