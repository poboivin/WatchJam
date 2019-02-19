using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

public class PierInputManager : MonoBehaviour
{
    public enum PlayerNumber { P1, P2, P3, P4, PC }
    public enum ButtonName {MoveHorizontal, MoveVertical, Fire1 , Fire2, Fire3 , Fire4, Rtrigger,Ltrigger, AimHorizontal, AimVertical,RBumper,LBumper }
    public PlayerNumber playerNumber;
    public Player player;

    List<bool> enableButtonList;
    static ButtonName[] disabledButtonsOnPopup = { ButtonName.MoveHorizontal, ButtonName.MoveVertical, ButtonName.Fire1, ButtonName.Fire2, ButtonName.Fire3, ButtonName.Rtrigger, ButtonName.Ltrigger };

    public void Awake()
    {
        enableButtonList = Enumerable.Repeat( true, Enum.GetValues( typeof( ButtonName ) ).Length ).ToList();
        player = ReInput.players.GetPlayer((int)playerNumber);
    }
    public float GetAxis( string axisName)
    {
        var buttonName = Enum.Parse( typeof( ButtonName ), axisName );
        if( enableButtonList[( int )buttonName] )
            return player.GetAxis(axisName);
        return 0.0f;
    }
    public float GetAxis(ButtonName axisName)
    {
        if( enableButtonList[( int )axisName] )
            return player.GetAxis( axisName.ToString() );
        
        return 0.0f;
    }
    public bool GetButton( ButtonName buttonName)
    {
        if( enableButtonList[( int )buttonName] )
            return player.GetButton(buttonName.ToString());
        return false;
    }

    public bool GetButtonDown( ButtonName buttonName)
    {
        if( enableButtonList[( int )buttonName] )
            return player.GetButtonDown(buttonName.ToString());
        return false;
    }
   
    public  bool GetButtonUp(ButtonName buttonName)
    {
        if( enableButtonList[( int )buttonName] )
            return player.GetButtonUp(buttonName.ToString());
        return false;
    }

    public void DisableButtonsOnPopup()
    {
        foreach( var button in disabledButtonsOnPopup)
        {
            enableButtonList[( int )button] = false;
        }
    }

    public void EnableButtonsOnPopup()
    {
        foreach( var button in disabledButtonsOnPopup )
        {
            enableButtonList[( int )button] = true;
        }
    }
}
