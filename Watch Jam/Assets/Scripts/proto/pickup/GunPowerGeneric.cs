using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

abstract class GunPowerGeneric : ISpecialGunPower
{
    public int numSpecialBullets { get; set; }
    public bool enableAbility { get; set; }
    public float damageMultiplier { get; set; }

    protected Gun gun;

    public GunPowerGeneric( Gun playerGun, int numBulletCount )
    {
        gun = playerGun;
        numSpecialBullets = numBulletCount;
        enableAbility = false;
        damageMultiplier = 1.0f;
    }

    // set all the changes/variables for the speical power.
    virtual public void Activate()
    {
        enableAbility = true;
    }

    // restore everthing back to original abilities of the normal gun.
    virtual public void Deactivate()
    {
        gun.RestoreRocket();
    }

    // calls FireBullet every time the player fires the bullet.
    virtual public void FireBullet( GameObject bullet )
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

    abstract public void Update();

}

