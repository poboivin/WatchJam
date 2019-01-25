using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindController : MonoBehaviour
{
    private TimeController myTimeController;
    [HideInInspector]
    public TimeBody myTimeBody;
    [HideInInspector]
    public Ammo myAmmo;
    [HideInInspector]
    public PierInputManager myInputManager;

    public PierInputManager.ButtonName Rewind;

    private void Awake()
    {
        myTimeController = GetComponent<TimeController>();
        myTimeBody = GetComponent<TimeBody>();
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
    }

    // Use this for initialization
    void Start () {
		
	}

    // this trigger variable checks if the time power activates here or somewhere else.
    private bool controllerTriggered = false;
	// Update is called once per frame
	void Update () {

        if( myTimeController.isRewinding == false &&
            controllerTriggered == false &&
            myInputManager.GetAxis( Rewind.ToString() ) > 0.6f && 
            ( myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits ) )
        {
            if( myTimeBody.pointsInTime.Count >= 60 )
            {
                myTimeController.StartRewind();
            }
            controllerTriggered = true;
        }

        if( myTimeController.isRewinding == true &&
            controllerTriggered == true &&
            myInputManager.GetAxis( Rewind.ToString() ) < 0.1f )
        {
            myTimeController.StopRewind();
            controllerTriggered = false;
        }

        // release trigger in case more than two rewind events happen at the same time.
        if( myTimeController.isRewinding == false && controllerTriggered == true )
            controllerTriggered = false;
    }
}
