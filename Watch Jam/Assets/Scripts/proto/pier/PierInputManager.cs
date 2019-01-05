using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PierInputManager : MonoBehaviour
{
    public enum PlayerNumber { P1, P2, P3, P4, PC }
    public enum ButtonName {MoveHorizontal, MoveVertical, Fire1 , Fire2, Fire3 , Fire4, Rtrigger,Ltrigger, AimHorizontal, AimVertical,RBumper,LBumper }
    public PlayerNumber playerNumber;
    public Player player;
    public void Awake()
    {
        player = ReInput.players.GetPlayer((int)playerNumber);
    }
    public float GetAxis( string axisName)
    {
        return player.GetAxis(axisName);
    }
 
    public bool GetButton( ButtonName buttonName)
    {
        return player.GetButton(buttonName.ToString());
    }

    public bool GetButtonDown( ButtonName buttonName)
    {
        return player.GetButtonDown(buttonName.ToString());
    }
   
    public  bool GetButtonUp(ButtonName buttonName)
    {
        return player.GetButtonUp(buttonName.ToString());
    }
}
