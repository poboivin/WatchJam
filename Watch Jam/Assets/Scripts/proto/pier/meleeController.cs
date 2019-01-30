using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeController : MonoBehaviour
{
    [HideInInspector]
    public PierInputManager myInputManager;
    [HideInInspector]
    public Animator myAnimator;
    MeleeWeapon MyMeleeWeapon;
    // Use this for initialization
    void Start () {
        myInputManager = gameObject.GetComponent<PierInputManager>();
        myAnimator = gameObject.GetComponentInChildren<Animator>();
        MyMeleeWeapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        MyMeleeWeapon.myOwner = gameObject.GetComponentInChildren<LifeSpan>();
    }

    // Update is called once per frame
    void Update () {
        // If the jump button is pressed and the player is grounded then the player should jump.
        if (myInputManager.GetButtonDown(Settings.c.MeleeButton))
        {
            myAnimator.SetTrigger("attack");
        }
    }
}
