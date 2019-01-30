using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TutorialHUD : MonoBehaviour {
    
    //HUD element references
    public Text TutorialMessage;
    public Image TutorialImage;
    public Sprite[] TutorialImages;
    private int tutorialTask = 0;
    
    //Game Object References
    public GameObject Teleporter;
    
    //inputs
    public PierInputManager inputManager;
    public PierInputManager.ButtonName Horizontal;
    public PierInputManager.ButtonName AButton;
    public PierInputManager.ButtonName XButton;
    public PierInputManager.ButtonName LT;
    public PierInputManager.ButtonName RT;

    //Tutorial stats
    private float TimesFirePressed;
    private float PowerStart;
    const float PowerTime = 1.5f;
   



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputChecker();
    }

    
    //Switches the tutorial message and image everytime this event is fired off
    public void SwitchTutorialMessage()
    {
       
        tutorialTask++;

        switch (tutorialTask)
        {
            case 1:
                TutorialMessage.text = "Use the left stick to move";
                TutorialImage.sprite = TutorialImages[1];


                break;

            case 2:
                TutorialMessage.text = "Use A to jump";
                TutorialImage.sprite = TutorialImages[0];

                break;

            case 3:
                TutorialMessage.text = "Use X to shoot and left stick to aim";
                TutorialImage.sprite = TutorialImages[2];

                break;

            case 4:

                TutorialMessage.text = "Use left trigger to rewind time and reload ammo.";
                TutorialImage.sprite = TutorialImages[3];
                
                break;

            case 5:

                TutorialMessage.text = "Use right trigger to stop time.";
                TutorialImage.sprite = TutorialImages[4];
                break;

            case 6:

                TutorialMessage.text = "Enter the portal to begin";
                TutorialImage.enabled = false;
                break;
        }
    }



    //checks for input of the current section of the tutorial.It then progresses the tutorial everytime the cases condition is met
    public void InputChecker()
    {
        switch (tutorialTask)
        {
            //When player presses A to spawn in it will switch tutorial message and add to the amount of players playing
            case 0:
                
                if (inputManager.GetButtonDown(AButton))
                {
                    SwitchTutorialMessage();
                    TutorialManager.PlayersPlaying++;
                }

                break;

                //when the player moves horizontaly switch to the next section
            case 1:
                if (inputManager.GetButtonDown(Horizontal))
                {
                    SwitchTutorialMessage();
                }

                break;

                //when the player jumps switch to the next section
            case 2:
                if (inputManager.GetButtonDown(AButton))
                {
                    SwitchTutorialMessage();
                }

                break;

                //when the player fires there weapon 2 or more times switch to the next section
            case 3:
                if (inputManager.GetButtonDown(XButton))
                {
                    TimesFirePressed++;

                    if (TimesFirePressed >= 2)
                    {
                        SwitchTutorialMessage();
                    
                    }

                    
                }

                break;

                //when time is rewound for X amount of seconds progress the tutorial
            case 4:
                if (inputManager.GetButtonDown(LT))
                {
                    PowerStart = Time.time;
                   
                }

                if (inputManager.GetButton(LT))
                {

                   
                    if(Time.time-PowerStart>= PowerTime)
                    {
                        SwitchTutorialMessage();
                    }


                }

                break;

                //when time is stopped for X amount of seconds progress the last part of the tutorial
            case 5:
                if (inputManager.GetButtonDown(RT))
                {
                    PowerStart = Time.time;

                }

                if (inputManager.GetButton(RT))
                {
                    if (Time.time-PowerStart >= PowerTime)
                    {
                        SwitchTutorialMessage();
                        Teleporter.GetComponent<TutorialTelaport>().TurnOnTelporter();

                    }
                }

                break;
        }
    }
}
