using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierInputManager : MonoBehaviour
{
    public enum PlayerNumber { P1, P2, P3, P4, PC }
    public enum ButtonName { Fire1 , Fire2, Fire3 ,Jump }
    public PlayerNumber playerNumber;

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

}
