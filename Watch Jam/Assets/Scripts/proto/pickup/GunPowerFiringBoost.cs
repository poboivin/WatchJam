using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerFiringBoost : ISpecialGunPower
{
    public int numSpecialBullets { get; set; }
    public bool enableAbility { get; set; }

    private Gun gun;
    private float oldFireRate;
    private float newFireRate;

    public GunPowerFiringBoost( Gun playerGun, float fireRate, int numBoostedBulletCount )
    {
        gun = playerGun;
        oldFireRate = playerGun.fireRate;
        newFireRate = fireRate;
        numSpecialBullets = numBoostedBulletCount;
        enableAbility = false;
    }

    public void Activate()
    {
        gun.fireRate = newFireRate;
        gun.SpecialBar.ToggleRapidFireBar( true );
        enableAbility = true;
    }

    public void Deactivate()
    {
        gun.fireRate = oldFireRate;
        gun.SpecialBar.ToggleRapidFireBar( false );
        gun.RestoreRocket();
    }

    public void FireBullet( GameObject bullet )
    {
        if( enableAbility )
        {
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

