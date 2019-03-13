using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerBulletBounce : GunPowerGeneric
{
    public GunPowerBulletBounce( Gun playerGun, PickupBounce shotInfo )
        : base( playerGun, shotInfo.bouncedBulletCount )
    {
    }

    public override void Activate()
    {
        base.Activate();
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( true );
    }

    public override void Deactivate()
    {
        base.Deactivate();

        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( false );
    }

    public override void Update()
    {
        if( enableAbility )
        {
            // TODO : needs to change this bar to the other effect
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }

}

