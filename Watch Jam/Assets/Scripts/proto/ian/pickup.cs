using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public enum pickUpType {
        health,
        grenade,
        shield,         // Owl
        fireBoost,      // Cat
        shotgun,        // Multiple bullets spread out 
        bounce,         // Bullet can bounce the wall
    }
    public pickUpType type;
    public Rigidbody2D Grenade;              // Prefab of the rocket.

    // Use this for initialization
    Vector3 start;
    Vector3 end;
	void Start ()
    {
        start = transform.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = LerpUtility.SineWave(Time.time, start, Vector3.up,0.2f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (type == pickUpType.grenade)
            {
                Ammo player = collision.GetComponent<Ammo>();
                if(player != null && player.enabled == true)
                {
                    player.Grenade = Grenade;
                }
            }
            else if( type == pickUpType.shield )
            {
                PickupShield player = collision.GetComponentInChildren<PickupShield>();
                if( player != null )
                {
                    player.RefreshShield();
                }
            }
            else if( type == pickUpType.fireBoost )
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if( playerGun != null )
                {
                    PickupFireBoost boost = gameObject.GetComponent<PickupFireBoost>();
                    if( boost != null )
                    {
                        playerGun.SetGunPower( new GunPowerFiringBoost( playerGun, boost.fireRate, boost.boostedBulletCount ) );
                    }
                }
            }
            else if( type == pickUpType.bounce )
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if( playerGun != null )
                {
                    PickupBounce bounceInfo = gameObject.GetComponent<PickupBounce>();
                    if( bounceInfo != null )
                    {
                        playerGun.SetGunPower( new GunPowerBulletBounce( playerGun, bounceInfo.bouncedBulletCount ) );
                    }
                }
            }
            else if( type == pickUpType.shotgun )
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if( playerGun != null )
                {
                    PickupShotgun shotgunInfo = gameObject.GetComponent<PickupShotgun>();
                    if( shotgunInfo != null )
                    {
                        playerGun.SetGunPower( new GunPowerShotgun( playerGun, shotgunInfo.numTotalShots, shotgunInfo.numOfBulletsInOneShot, shotgunInfo.angle ) );

                    }
                }
            }

            Destroy( gameObject);
            //play pickup sound
        }

    }
}
