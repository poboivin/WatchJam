using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMelee : MonoBehaviour
{
    [HideInInspector]
    public PierInputManager myInputManager;
    public PierInputManager.ButtonName Melee;
    [HideInInspector]
    public LifeSpan myLifeSpan;
    [HideInInspector]
    public TimeController myTimeController;
    [HideInInspector]
    public TimeStopController myTimeStopController;
    [HideInInspector]

    public TimeRewindController myTimeRewindController;

    PlayerControl myPlayerControl;
    public float VelocityMagnitude = 50;
    public float Duration = 1;
    public float conter;
    public bool active = false;
    public float leftOverFactor = .33f;
    Vector3 dir = Vector3.right;
    Vector3 oldScale;
    // Start is called before the first frame update
    void Start()
    {
        myInputManager = GetComponent<PierInputManager>();
        myLifeSpan = GetComponent<LifeSpan>();
        myTimeController = GetComponent<TimeController>();
        myPlayerControl = GetComponent<PlayerControl>();
        myTimeStopController = GetComponent<TimeStopController>();
        myTimeRewindController = GetComponent<TimeRewindController>();
        oldScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (active == false && conter == 0 && myInputManager.GetButtonDown(Melee))
        {
            active = true;
            myLifeSpan.SetInvincibility(true);
            if (myTimeController.isStopped)
            {
                myTimeController.StopTimeStop();
            }
            if (myTimeController.isRewinding)
            {
                myTimeController.StopRewind() ;

            }
            myTimeStopController.enabled = false;
            myTimeRewindController.enabled = false ;
            dir = new Vector2(myTimeController.myInputManager.GetAxis(Settings.c.MoveXAxis), myTimeController.myInputManager.GetAxis(Settings.c.MoveYAxis));
            if (dir.sqrMagnitude == 0)
            {
                if (myPlayerControl.facingRight == false)
                {
                    dir = -Vector3.right;
                }
                else
                {
                    dir = Vector3.right;
                }
            }
            


            oldScale = transform.localScale;
            myTimeController.myRigidbody2D.velocity = dir.normalized * VelocityMagnitude;

        }
        else if (active == true && myTimeController.isStopped == false)
        {

            myTimeController.myRigidbody2D.velocity = dir.normalized * VelocityMagnitude;
            conter += Time.deltaTime;
            transform.localScale = new Vector3(.7f, .2f, 1);

            if (conter >= Duration)
            {
                transform.localScale = oldScale;

                active = false;
                myLifeSpan.SetInvincibility(false);
                myTimeStopController.enabled = true;
                myTimeRewindController.enabled = true;
                myTimeController.myRigidbody2D.velocity *= leftOverFactor;
            }
        }
        else
        {
            active = false;
            conter = 0;
        }
    }
}
