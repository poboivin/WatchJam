using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public int MaxAmmo = 3;
    public int CurrentAmmo;
   // public bool HasGrenade = false;
    [HideInInspector]
    public Rigidbody2D Grenade;              // Prefab of the rocket.  changed by the pick up script 
}
