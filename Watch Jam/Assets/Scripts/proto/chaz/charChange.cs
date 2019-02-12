using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charChange : MonoBehaviour {

	public float dPadHorizontal;
	public float dPadVertical;
	PierInputManager myInputManager;
	SpriteRenderer spr;
	Animator ani;

	public AnimatorOverrideController a1;
	public RuntimeAnimatorController a2;
	public AnimatorOverrideController a3;
	public AnimatorOverrideController a4;


	private int onChar = 1;
	private string sHor;
	private string sVer;


	// Use this for initialization
	void Awake () {
		spr = transform.Find ("body/Army Lady/Sprite").GetComponent<SpriteRenderer> ();
		ani = transform.Find ("body/Army Lady/Sprite").GetComponent<Animator> ();
		myInputManager = gameObject.GetComponent<PierInputManager>();
		SpriteRenderer s = transform.Find ("body/Army Lady/Sprite").GetComponent<SpriteRenderer> ();

		if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P1) {
			s.color = new Color (0.8f, 0.0f, 0.0f, 1.0f); 
			sHor = "P1Horizontal";
			sVer = "P1Vertical";
		}
		else if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P2) {
			s.color = new Color (0.0f, 0.0f, 0.8f, 1.0f); 
			sHor = "P2Horizontal";
			sVer = "P2Vertical";
		}
		else if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P3) {
			s.color = new Color (0.0f, 0.8f, 0.0f, 1.0f); 
			sHor = "P3Horizontal";
			sVer = "P3Vertical";
		}
		else if (myInputManager.playerNumber == PierInputManager.PlayerNumber.P4) {
			s.color = new Color (0.8f, 0.8f, 0.0f, 1.0f); 
			sHor = "P4Horizontal";
			sVer = "P4Vertical";
		}


	}
	
	// Update is called once per frame
	void Update () {
		dPadHorizontal = Input.GetAxis (sHor);
		dPadVertical = Input.GetAxis (sVer);

		if (dPadHorizontal > 0 && onChar != 1) {

			ani.runtimeAnimatorController = a1;
			onChar = 1;

		} else if (dPadHorizontal < 0 && onChar != 3) {

			ani.runtimeAnimatorController = a3;
			onChar = 3;

		}

		if (dPadVertical > 0 && onChar != 4) {

			ani.runtimeAnimatorController = a4;
			onChar = 4;


		} else if (dPadVertical < 0 && onChar != 2) {

			ani.runtimeAnimatorController = a2;
			onChar = 2;


		}
	}
}
