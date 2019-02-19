using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Get Time Stop Features from Time Controller
public class TimeStopController : MonoBehaviour
{
    private TimeController myTimeController;
    [HideInInspector]
    public Ammo myAmmo;
    [HideInInspector]
    public PierInputManager myInputManager;
    public PierInputManager.ButtonName TimeStop;

    void Awake()
    {
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
        myTimeController = GetComponent<TimeController>();
    }

    // Use this for initialization
    void Start () {
		
	}

    // this trigger variable checks if the time power activates here or somewhere else.
    private bool controllerTriggered = false;
    // Update is called once per frame
    void Update()
    {
        if( myTimeController.isStopped == false &&
            controllerTriggered == false &&
            myInputManager.GetAxis( TimeStop ) > 0.5f && 
            ( myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits ) )
        {
            myTimeController.StartTimeStop();
            controllerTriggered = true;
        }
        if( myTimeController.isStopped == true &&
            controllerTriggered == true &&
            myInputManager.GetAxis( TimeStop ) < 0.1f )
        {
            myTimeController.StopTimeStop();
            controllerTriggered = false;
        }

        // release trigger in case more than two stop events happen at the same time.
        if( myTimeController.isStopped == false && controllerTriggered == true )
            controllerTriggered = false;

    }
}
