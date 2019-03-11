using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class GunPowerPierce : ISpecialGunPower
{
    public int numSpecialBullets { get; set; }
    public bool enableAbility { get; set; }

    private Gun gun;

    public GunPowerPierce( Gun playerGun, PickupPierce pickupInfo )
    {
        gun = playerGun;
        numSpecialBullets = pickupInfo.piercingBulletCount;
        enableAbility = false;
    }

    // set all the changes/variables for the speical power.
    public void Activate()
    {
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( true );
        // TODO : may need to change the rocket prefab for this ability
        enableAbility = true;
    }

    // restore everthing back to original abilities of the normal gun.
    public void Deactivate()
    {
        // TODO : needs to change this bar to the other effect
        gun.SpecialBar.ToggleRapidFireBar( false );
        gun.RestoreRocket();
    }

    // calls FireBullet every time the player fires the bullet.
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

    // calls Update every frame while the special power is equipped.
    public void Update()
    {
        if( enableAbility )
        {
            // TODO : needs to change this bar to the other effect
            gun.SpecialBar.SetRapidBarFill( numSpecialBullets );
        }
    }
}

