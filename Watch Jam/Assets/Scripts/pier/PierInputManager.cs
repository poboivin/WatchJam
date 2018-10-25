using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierInputManager : MonoBehaviour
{
    public enum PlayerNumber { P1, P2, P3, P4, PC }
    public enum ButtonName { Fire1 , Fire2, Fire3 ,Jump,Rtrigger,Ltrigger }
    public PlayerNumber playerNumber;

    public float GetAxis( string axisName)
    {
        if (playerNumber != PlayerNumber.PC)
        {
            return Input.GetAxis(playerNumber.ToString() + axisName);

        }
        else
        {
            return Input.GetAxis(axisName);

        }
    }
    public static float GetAxis(PlayerNumber player ,string axisName)
    {
        if (player != PlayerNumber.PC)
        {
            return Input.GetAxis(player.ToString() + axisName);

        }
        else
        {
            return Input.GetAxis(axisName);

        }
    }
    public static bool GetButton(PlayerNumber player, ButtonName buttonName)
    {
        if (buttonName == ButtonName.Rtrigger || buttonName == ButtonName.Ltrigger)
        {
            float input = Input.GetAxis(player.ToString() + buttonName.ToString());
            Debug.Log(input);
            if (input == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (player == PlayerNumber.PC)
        {
                        return Input.GetButton(buttonName.ToString());

        }
        else
        {
            return Input.GetButton(player.ToString() + buttonName.ToString());


        }
    }
    public static bool GetButtonDown(PlayerNumber player, ButtonName buttonName)
    {
        if (player != PlayerNumber.PC)
        {
            return Input.GetButtonDown(player.ToString() + buttonName.ToString());

        }
        else
        {
            return Input.GetButtonDown(buttonName.ToString());

        }
    }
    public bool GetButtonDown( ButtonName buttonName)
    {
        if (playerNumber != PlayerNumber.PC)
        {
            return Input.GetButtonDown(playerNumber.ToString() + buttonName.ToString());

        }
        else
        {
            return Input.GetButtonDown(buttonName.ToString());

        }
    }
    public static bool GetButtonUp(PlayerNumber player, ButtonName buttonName)
    {
        if (player != PlayerNumber.PC)
        {
            return Input.GetButtonUp(player.ToString() + buttonName.ToString());

        }
        else
        {
            return Input.GetButtonUp(buttonName.ToString());

        }
    }
    public  bool GetButtonUp(ButtonName buttonName)
    {
        if (playerNumber != PlayerNumber.PC)
        {
            return Input.GetButtonUp(playerNumber.ToString() + buttonName.ToString());

        }
        else
        {
            return Input.GetButtonUp(buttonName.ToString());

        }
    }
}
