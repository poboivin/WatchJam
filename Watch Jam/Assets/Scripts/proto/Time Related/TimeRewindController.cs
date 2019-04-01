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
	public AfterImage myAfterImage;
	public bool useNewRewind = true;

	private bool canShowTrail = true;


   // public PierInputManager.ButtonName Rewind;

    private void Awake()
    {
        myTimeController = GetComponent<TimeController>();
        myTimeBody = GetComponent<TimeBody>();
        myAmmo = GetComponent<Ammo>();
        myInputManager = GetComponent<PierInputManager>();
		myAfterImage  = GetComponent<AfterImage>();
    }

    // Use this for initialization
    void Start () {
		
	}

    // this trigger variable checks if the time power activates here or somewhere else.
    private bool controllerTriggered = false;
	// Update is called once per frame
	void Update () {
		
			if (myTimeController.isRewinding == false &&
			    myInputManager.GetAxis (Settings.c.Rewind) > 0.6f &&
			    canShowTrail &&
			    (myAmmo.CurrentAmmo < myAmmo.MaxAmmo || Settings.s.noLimits)) {

                if (myTimeBody.pointsInTime.Count >= 1)
                      myAfterImage.DrawLine();

				controllerTriggered = true;
			}

			if (myInputManager.GetAxis (Settings.c.TimeStop) > 0.5f) {
                myAfterImage.DisableGhost();
                canShowTrail = false;
				controllerTriggered = false;
			}
			if (controllerTriggered == true &&
			    myInputManager.GetAxis (Settings.c.Rewind) < 0.1f) {
			
				myAfterImage.DisableGhost ();
				//myTimeController.StartRewind();
				myTimeBody.doInstantRewind ();
				controllerTriggered = false;
			}




			if (myInputManager.GetAxis (Settings.c.Rewind) < 0.1f && myInputManager.GetAxis (Settings.c.TimeStop) < 0.1f) {
				canShowTrail = true;
			}



       

			// release trigger in case more than two rewind events happen at the same time.
			if (myTimeController.isRewinding == false && controllerTriggered == true) {

			}
		
		
    }
}
