using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialBarController : MonoBehaviour
{
    public GameObject RapidFireBar;
    public Image RapidFireFill;

    public GameObject StasisBar;
    public Image StatisFill;

    public Color[] AmmoBarFills;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetRapidBarFill(float AmountLeft)
    {
//        Debug.Log(AmountLeft);
        RapidFireFill.fillAmount = AmountLeft / 10;

        //Set Bar Color

    }


    public void ToggleRapidFireBar(bool RapidFireMode)
    {
        if (RapidFireMode)
        {
            RapidFireBar.SetActive(true);
        }

        else
        {
            RapidFireBar.SetActive(false);
        }

    }

    public void ChangeBarColor(int C)
    {
        RapidFireFill.color = AmmoBarFills[C];
        
        
    }



}
