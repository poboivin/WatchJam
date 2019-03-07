using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Gun : MonoBehaviour, IGun
{
    private ISpecialGunPower specialGunPower;
    
    public void SetGunPower( ISpecialGunPower newGunPower )
    {
        if( specialGunPower != null )
        {
            specialGunPower.Deactivate();
        }
        specialGunPower = newGunPower;
        specialGunPower.Activate();
    }

    public void FireBullet( GameObject bullet )
    {
        if( specialGunPower != null )
        {
            specialGunPower.FireBullet( bullet );
            if( specialGunPower.enableAbility == false )
            {
                specialGunPower.Deactivate();
                specialGunPower = null;
            }
        }
    }

    public void RestoreRocket()
    {
        rocket = OriginalRocket;
    }

}
