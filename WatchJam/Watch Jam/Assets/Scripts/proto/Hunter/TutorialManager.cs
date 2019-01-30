using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    public static int PlayersReadied;
    public static int PlayersPlaying;
    public static bool Active;

    // Use this for initialization
    void Start () {
        //makes sure all the variables are reset
        PlayersReadied = 0;
        PlayersPlaying = 0;
        Active = false;
	}
	
	// Update is called once per frame
	void Update () {
  
	}

    //if the amount of players who are readied is >= the amount playing start the game
    public void LoadLevel()
    {
       
        if(PlayersReadied>= PlayersPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
