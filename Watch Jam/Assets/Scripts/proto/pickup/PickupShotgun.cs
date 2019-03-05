using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShotgun : MonoBehaviour
{
    [SerializeField]
    public int numTotalShots = 8;

    /// <summary>
    /// number of bullets in a single shot
    /// </summary>
    [SerializeField]
    public int numOfBulletsInOneShot = 4;

    /// <summary>
    /// the range of launch angle for all shots, will be evenly spread out from -angle to +angle.
    /// </summary>
    [SerializeField]
    public float angle = 30.0f;

}
