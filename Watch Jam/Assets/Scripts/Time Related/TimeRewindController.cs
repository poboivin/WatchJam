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
	
	// Update is called once per frame
	void Update () {
        if( myInputManager.GetAxis( Rewind.ToString() ) > 0.01f )
        {
            myTimeController.myAfterImage.DrawLine();
        }

        if( myTimeController.isRewinding == false && 
            myInputManager.GetAxis( Rewind.ToString() ) > 0.6f && 
            ( myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits ) )
        {
            if( myTimeBody.pointsInTime.Count >= 60 )
                myTimeController.StartRewind();
        }

        if( myTimeController.isRewinding == true && 
            myInputManager.GetAxis( Rewind.ToString() ) < 0.1f )
        {
            myTimeController.StopRewind();
        }
    }
}
