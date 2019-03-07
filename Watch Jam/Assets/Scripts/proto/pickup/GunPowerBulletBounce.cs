using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerBulletBounce : ISpecialGunPower
{
    public int numSpecialBullets { get; set; }
    public bool enableAbility { get; set; }

    private Gun gun;

    public GunPowerBulletBounce( Gun playerGun, int numBoostedBulletCount )
    {
        gun = playerGun;
        numSpecialBullets = numBoostedBulletCount;
        enableAbility = false;
    }

    public void Activate()
    {
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( true );
        // TODO : may need to change the rocket prefab for this ability
        enableAbility = true;
    }

    public void Deactivate()
    {
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( false );
        gun.RestoreRocket();
    }

    public void FireBullet( GameObject bullet )
    {
        if( enableAbility )
        {
            if( bullet != null )
            {
                bullet.GetComponent<Rocket>().enableBounce = true;
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
            // TODO : needs to change this bar to the other effect
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }

}

