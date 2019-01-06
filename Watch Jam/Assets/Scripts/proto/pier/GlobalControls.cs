using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class GlobalControls : ScriptableObject
{
    [Header("Shoot")]
    [Tooltip("main shooting btn")]
    public PierInputManager.ButtonName ShootButton;   //button to shoot
    [Tooltip("alternate shooting btn")]
    public PierInputManager.ButtonName AltShootButton;   //button to shoot

    [Header("Aim")]
    public PierInputManager.ButtonName MainAimXAxis;
    public PierInputManager.ButtonName MainAimYAxis;
    public PierInputManager.ButtonName AltAimXAxis;
    public PierInputManager.ButtonName AltAimYAxis;

    [Header("Movement")]
    public PierInputManager.ButtonName MoveXAxis;
    public PierInputManager.ButtonName MoveYAxis;
    public PierInputManager.ButtonName jumpButton;
    public PierInputManager.ButtonName AltjumpButton;
    [Header("Time Power")]
    public PierInputManager.ButtonName TimeStop;
    public PierInputManager.ButtonName Rewind;

    [Header("Other")]
    public PierInputManager.ButtonName MeleeButton;


}
