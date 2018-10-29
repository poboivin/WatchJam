using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GlobalSettings : ScriptableObject
{

    public bool noLimits;
    public float gunKnockBack = 20f;
    public float bulletKnockBack = 10f;
    public float bulletDamage = 4f;
    [Range (0,1)]
    public float airControl = 0;
    [Header("velocity")]
    public bool timeStopStore = true;
    public bool timeStopKillVelocity = true;
    public bool rewindKillVelocity = true;

    [Header("life related")]
    public bool lifeDecay;
    public bool lifeSteal = false;
    public bool rewindInvincibility = false;
    [Header("ammo regen")]
    public bool timeStopAmmoRegen = false;
    public bool rewindAmmoRegen = false;
    public bool passiveAmmoRegen = true;

    public bool TwiceAsFastRewind = false;
}
