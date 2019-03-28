using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerFiringBoost : GunPowerGeneric
{
    private float oldFireRate;
    private float newFireRate;

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
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gun.fireRate = oldFireRate;
        gun.SpecialBar.ToggleRapidFireBar( false );
    }

    public override void Update()
    {
        if( enableAbility )
        {
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }
}

