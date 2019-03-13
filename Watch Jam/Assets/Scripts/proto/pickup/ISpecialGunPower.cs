using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface ISpecialGunPower
{
    // if the special power is based on the number of bullets.
    int numSpecialBullets { get; set; }

    // to check if the special power is fully consumed or not.
    bool enableAbility { get; set; }

    // increase/decrease the damage of the bullet( default : 1.0 )
    float damageMultiplier { get; set; }

	float knockbackMultiplier { get; set; }

    // set all the changes/variables for the speical power.
    void Activate();

    // restore everthing back to original abilities of the normal gun.
    void Deactivate();

    // calls FireBullet every time the player fires the bullet.
    void FireBullet( GameObject bullet );

    // calls Update every frame while the special power is equipped.
    void Update();
}

