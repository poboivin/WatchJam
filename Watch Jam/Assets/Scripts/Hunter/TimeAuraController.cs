using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAuraController : MonoBehaviour {
    //References to all colors of time aura's
    //the 0 index of the arrays are the front sprite
    //the 1 index of the arrays are the back sprite
    public Sprite[] Green;
    public Sprite[] Orange;
    public Sprite[] Purple;
    public Sprite[] Red;
    public GameObject FrontRing;
    public GameObject BackRing;







    private void Start()
    {
        TurnOffAura();
        
    }


    //turns on the aura of the color given
    public void TurnOnAura (string color)
    {
        // a switch that controls what color is turned on
        switch(color)
        {
            case "green":
                FrontRing.GetComponent<SpriteRenderer>().sprite = Green[0];
                BackRing.GetComponent<SpriteRenderer>().sprite = Green[1];



                break;

            case "orange":

                FrontRing.GetComponent<SpriteRenderer>().sprite = Orange[0];
                BackRing.GetComponent<SpriteRenderer>().sprite = Orange[1];

                break;

            case "purple":

                FrontRing.GetComponent<SpriteRenderer>().sprite = Purple[0];
                BackRing.GetComponent<SpriteRenderer>().sprite = Purple[1];

                break;

            case "red":
                FrontRing.GetComponent<SpriteRenderer>().sprite = Red[0];
                BackRing.GetComponent<SpriteRenderer>().sprite = Red[1];
                break;

        }
    }



    //turns off the time aura
    public void TurnOffAura()
    {
        FrontRing.GetComponent<SpriteRenderer>().sprite = null;
        BackRing.GetComponent<SpriteRenderer>().sprite = null;
    }



}
