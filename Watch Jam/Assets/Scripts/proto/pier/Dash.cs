using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    /*
     when is paused if you let go with a direction input 
     set velocity in that direction  every frame for duration until collision 
     
     
         
         
         */



    [HideInInspector]
    public TimeController myTimeController;
    public TimeStopController myTimeStopController;
    public TimeRewindController myTimeRewindController;
    public float VelocityMagnitude = 1;
    public float Duration = 1;
    public float conter;
    public bool active = false;
    Vector2 dir;
    public float leftOverFactor = .33f;
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;          // Whether or not the player is grounded.
   

    // Use this for initialization
    void Start ()
    {
        myTimeController = GetComponent<TimeController>();
        myTimeStopController = GetComponent<TimeStopController>();
        myTimeRewindController = GetComponent<TimeRewindController>();
        groundCheck = transform.Find("groundCheck");

    }

    // Update is called once per frame
    void Update ()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if(grounded)
        {
            conter = 0;
            active = false;
        }


        if (active == false && conter == 0 &&   myTimeController.myInputManager.GetButtonUp(Settings.c.TimeStop) )
        {
            dir = new Vector2(myTimeController.myInputManager.GetAxis(Settings.c.MoveXAxis), myTimeController.myInputManager.GetAxis(Settings.c.MoveYAxis));


            if (dir.SqrMagnitude() != 0)
            {
                active = true;
                //
            }
        }
        else if ( active == true && myTimeController.isStopped == false)
        {
           
            myTimeController.myRigidbody2D.velocity = dir.normalized * VelocityMagnitude;
            conter += Time.deltaTime;

            if(conter >= Duration)
            {
                active = false;
                myTimeController.myRigidbody2D.velocity *= leftOverFactor;
            }
        }
        else
        {
            active = false;
        }
	}
   
}
