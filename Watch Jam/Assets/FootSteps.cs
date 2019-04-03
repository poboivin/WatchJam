using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{

    public SoundBite[] footsteps;

    public AudioSource AS;


    public void playFootSteps()
    {
        int i = Random.Range(0, footsteps.Length); 
        AS.PlayOneShot(footsteps[i].clip);

    }
}
