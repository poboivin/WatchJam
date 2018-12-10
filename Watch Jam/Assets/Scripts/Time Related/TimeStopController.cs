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

    // Update is called once per frame
    void Update()
    {
        if( myTimeController.isStopped == false && 
            myInputManager.GetAxis( TimeStop.ToString() ) > 0.5f && 
            ( myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits ) )
        {
            myTimeController.StartTimeStop();
        }
        if( myTimeController.isStopped == true && 
            myInputManager.GetAxis( TimeStop.ToString() ) < 0.1f )
        {
            myTimeController.StopTimeStop();
        }
    }
}
