using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Normal,
    TimeLimit,
};

[CreateAssetMenu]
public class GlobalSettings : ScriptableObject
{

    public bool noLimits;
    public bool unlimitedAmmo = true;
    public float gunKnockBack = 20f;
    public float bulletKnockBack = 10f;
    public float bulletDamage = 4f;
    public float meleeDamage = 4f;
    public float meleeKnockBack = 10f;
    public float fireRate = 0.25f;

    public float totalLife = 60;
    public bool stopTimeStoreBullet = false;
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
    public float rewindAmmoFactor = 4f;
    public float passiveAmmoFactor = 1f;
    public float stopTimeAmmoFactor = 1f;

    public bool TwiceAsFastRewind = false;
    public bool ImmobileRecord = true;

    [Header( "Game Mode" )]
    public GameMode gameMode = GameMode.Normal;
    public float playerRespawnTime = 3.0f;
    public float timeLimitInSceonds = 120.0f;
}
