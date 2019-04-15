using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class GunPowerTimedBomb : GunPowerGeneric
{
    float timeOut;
    float rangeScale;

    public GunPowerTimedBomb(Gun playerGun, PickupTimedBomb bombInfo)
        : base(playerGun, bombInfo.timedBombCount)
    {
        timeOut = bombInfo.timeOutDuration;
        rangeScale = bombInfo.explosionRangeScale;
    }

    public override void Activate()
    {
        base.Activate();
        gun.SpecialBar.ToggleRapidFireBar( true );
        gun.SpecialBar.ChangeBarColor(5);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gun.SpecialBar.ToggleRapidFireBar( false );
    }

    public override void FireBullet(GameObject bullet)
    {
        base.FireBullet(bullet);

        RocketTimedBomb bomb = bullet.GetComponent<RocketTimedBomb>();
        bomb.TimeAlive = timeOut;
        bomb.SetExplosionRangeScale( rangeScale );
    }

    public override void Update()
    {
        if (enableAbility)
        {
            gun.SpecialBar.SetRapidBarFill(numSpecialBullets);
        }
    }

}

