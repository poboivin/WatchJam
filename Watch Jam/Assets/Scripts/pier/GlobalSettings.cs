using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GlobalSettings : ScriptableObject
{

    public bool noLimits;
    public bool lifeDecay;
    public float gunKockBack = 20f;
    public bool airControl = false;
    public bool timeStopStore = true;
    public bool timeStopKillVelocity = true;
}
