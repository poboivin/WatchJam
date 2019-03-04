using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        gun.SpecialBar.ToggleRapidFireBar( true );
        enableAbility = true;
    }

    public void Deactivate()
    {
        gun.SpecialBar.ToggleRapidFireBar( false );
        gun.RestoreRocket();
    }

    public void FireBullet( Rocket bullet )
    {
        if( bullet != null )
        {
            bullet.enableBounce = true;
        }

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

