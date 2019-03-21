using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject OwnedUI;
    public Image HealthBar;
    public LifeSpan Health;
    public Ammo AmmoAmount;


    // Start is called before the first frame update
    void Start()
    {
        OwnedUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
