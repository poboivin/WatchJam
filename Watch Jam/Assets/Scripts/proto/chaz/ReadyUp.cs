using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUp : MonoBehaviour
{
	
	public bool isReady = false;
	PierInputManager myInputManager;
	public GameObject g;

	void Awake(){
		//g = this.gameObject.Find ("Ready");
		myInputManager = gameObject.GetComponent<PierInputManager>();

	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Joystick1Button3))
		{
			if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P1) 
			{
				g.SetActive (!isReady);
				isReady = !isReady;
			}
		}
		if (Input.GetKeyDown(KeyCode.Joystick2Button3))
		{
			if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P2) 
			{
				g.SetActive (!isReady);
				isReady = !isReady;
			}
		}
		if (Input.GetKeyDown(KeyCode.Joystick3Button3))
		{
			if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P3)
			{
				g.SetActive (!isReady);
				isReady = !isReady;
			}
		}
		if (Input.GetKeyDown(KeyCode.Joystick4Button3))
		{
			if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P4) 
			{
				g.SetActive (!isReady);
				isReady = !isReady;
			}
		}


    }
}
	