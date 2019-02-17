using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class TimeEffectUIController : MonoBehaviour
{
    public GameObject StopTimeUI;
    public GameObject RewindTimeUI;
    public AnimationClip Rewindstart;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleStoppedTime(bool isturningOn)
    {
        //Checks if time is being stopped or unstopped

        if (isturningOn == true)
        {
            StopTimeUI.SetActive(true);
            StopTimeUI.GetComponent<Animator>().Play("StopTime");
        }


        else
        {
            StopTimeUI.SetActive(false);
        }
    }



    public void ToggleRewindTime(bool isrewinding)
    {
        if (isrewinding)
        {
            RewindTimeUI.SetActive(true);
            StartCoroutine(HandleRewindAnimation());
        }

        else
        {
            RewindTimeUI.SetActive(false);
            StopCoroutine(HandleRewindAnimation());
        }

    }



    IEnumerator HandleRewindAnimation()
    {
        Animator RewindAnimator = RewindTimeUI.GetComponent<Animator>();
        //Plays the rewind start
        RewindAnimator.Play("RewindStart");
        
        yield return new WaitForSeconds(Rewindstart.length);
        //Once rewind start is complete play rewind Loop
        RewindAnimator.Play("RewindLoop");
        //Stop running coroutine once finished

        
        


    }

}
