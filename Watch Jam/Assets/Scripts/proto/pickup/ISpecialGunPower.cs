using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISpecialGunPower
{
    int numSpecialBullets { get; set; }
    bool enableAbility { get; set; }

    void Activate();
    void Deactivate();
    // may need to have Bullet Interface for thsi later.
    void FireBullet( Rocket bullet );
    void Update();
}

