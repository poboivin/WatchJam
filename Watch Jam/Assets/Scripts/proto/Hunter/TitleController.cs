using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

    public PierInputManager[] inputManager;
    public PierInputManager.ButtonName AButton;





    // Use this for initialization
    void Start () {
  
	}
	
	// Update is called once per frame
	void Update () {
        if (inputManager[0].GetButtonDown(AButton) ||
      inputManager[1].GetButtonDown(AButton) ||
      inputManager[2].GetButtonDown(AButton) ||
      inputManager[3].GetButtonDown(AButton))
        {
            Destroy(gameObject);
        }
    }
}
