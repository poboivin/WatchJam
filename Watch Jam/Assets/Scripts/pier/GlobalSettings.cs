using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GlobalSettings : ScriptableObject
{

    public bool noLimits;
    public bool lifeDecay;
    public float gunKockBack = 20f;
    [Range (0,1)]
    public float airControl = 0;
    public bool timeStopStore = true;
    public bool timeStopKillVelocity = true;
    public bool timeStopAmmoRegen = false;
    public bool rewindKillVelocity = true;
    public bool lifeSteal = false;
    public bool rewindInvincibility = false;
}
