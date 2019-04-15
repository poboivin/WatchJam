using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GunPowerThicc : GunPowerGeneric
{
    public GunPowerThicc( Gun playerGun, PickupThicc pickupInfo )
        : base( playerGun, pickupInfo.thiccBulletCount )
    {
        damageMultiplier = pickupInfo.damageMultiplier;
		knockbackMultiplier = pickupInfo.knockbackMultiplier;
    }

    // set all the changes/variables for the speical power.
    public override void Activate()
    {
        base.Activate();

        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( true );
        gun.SpecialBar.ChangeBarColor(3);
    }

    // restore everthing back to original abilities of the normal gun.
    public override void Deactivate()
    {
        base.Deactivate();

        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( false );
    }

    public override void FireBullet( GameObject bullet )
    {
        base.FireBullet( bullet );
        BulletLeach bulletInfo = bullet.GetComponent<BulletLeach>();

        if( bulletInfo != null )
        {
            bulletInfo.damageMultiplier = damageMultiplier;
			bulletInfo.knockbackMultiplier = knockbackMultiplier;
        }
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
