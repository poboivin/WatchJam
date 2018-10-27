using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour {
    public int MaxAmmo = 6;
    public int CurrentAmmo;
    public bool HasGrenade = false;

    public Image Display;

    // Use this for initialization
    void Start () {
        CurrentAmmo = MaxAmmo;
	}
	
	// Update is called once per frame
	void Update () {
        Display.fillAmount = (float)CurrentAmmo / (float)MaxAmmo;
       
    }
}
