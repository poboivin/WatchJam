using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MakeShiftLevelSelector : MonoBehaviour
{
    bool IsPaused = false;
    public GameObject JapanButton;
    public GameObject FuturisticButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("pressedSpace");
            if (!IsPaused)
            {
                IsPaused = true;
                ToggleUI(true);
               
               
            }

            else
            {
                IsPaused = false;
                ToggleUI(false);
            }
        }
    }



    public void LoadJapanLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadFuturisticLevel()
    {
        SceneManager.LoadScene(1);
    }


    void ToggleUI(bool Ison)
    {
        if (Ison)
        {
            
            GetComponent<Image>().enabled = true;
            JapanButton.SetActive(true);
            FuturisticButton.SetActive(true);
        }

        else
        {
            GetComponent<Image>().enabled = false;
            JapanButton.SetActive(false);
            FuturisticButton.SetActive(false);
        }
    }

}
