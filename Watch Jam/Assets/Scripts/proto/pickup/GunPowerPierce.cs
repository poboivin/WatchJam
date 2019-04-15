using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class GunPowerPierce : GunPowerGeneric
{
    public GunPowerPierce( Gun playerGun, PickupPierce pickupInfo )
        : base( playerGun, pickupInfo.piercingBulletCount )
    {
    }

    // set all the changes/variables for the speical power.
    public override void Activate()
    {
        base.Activate();
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( true );
        gun.SpecialBar.ChangeBarColor(1);
    }

    // restore everthing back to original abilities of the normal gun.
    public override void Deactivate()
    {
        base.Deactivate();
        
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( false );
    }

    // calls Update every frame while the special power is equipped.
    public override void Update()
    {
        if( enableAbility )
        {
            // TODO : needs to change this bar to the other effect
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }
}

