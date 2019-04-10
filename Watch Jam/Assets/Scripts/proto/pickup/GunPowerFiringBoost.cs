using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

class GunPowerFiringBoost : GunPowerGeneric
{
    private float oldFireRate;
    private float newFireRate;
    bool startFiring = false;
    GameObject bulletPrefab;
    Vector3 firingPosition;
    Quaternion firingRotation;
    Vector2 bulletVelocity;
    float firingTimer;

    public GunPowerFiringBoost( Gun playerGun, PickupFireBoost shotInfo )
        : base( playerGun, shotInfo.boostedBulletCount )
    {
        oldFireRate = Settings.s.fireRate;
        newFireRate = shotInfo.fireRate;
    }

    public override void Activate()
    {
        base.Activate();
        gun.fireRate = newFireRate;
        gun.SpecialBar.ToggleRapidFireBar( true );
        Debug.Log( "Activate" );
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gun.fireRate = oldFireRate;
        gun.SpecialBar.ToggleRapidFireBar( false );
        Debug.Log( "Deactivate" );
    }

    public override void Update()
    {
        if( enableAbility )
        {
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );

            if( startFiring )
            {
                firingTimer += Time.deltaTime;
                if( firingTimer > newFireRate )
                {
                    GameObject newBullet = UnityEngine.Object.Instantiate( bulletPrefab, gun.gameObject.transform.position, firingRotation );
                    if( newBullet )
                    {
                        bulletPrefab = newBullet;
                        newBullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
                    }
                    if( --numSpecialBullets == 0 )
                    {
                        enableAbility = false;
                        startFiring = false;
                    }
                    firingTimer = 0.0f;
                }
            }
        }
    }

    public override void FireBullet( GameObject bullet )
    {
        if( enableAbility )
        {
            numSpecialBullets--;
            bulletPrefab = bullet;
            bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
            firingPosition = bullet.transform.position;
            firingRotation = bullet.transform.rotation;
            startFiring = true;
            firingTimer = 0.0f;
        }
    }
    
}

