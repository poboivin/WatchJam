using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerReadyCheck : MonoBehaviour
{
	GameObject[] players;
	int playersReady = 0;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
		players = GameObject.FindGameObjectsWithTag ("Player");

		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetComponentInChildren<ReadyUp> ().isReady) {
				playersReady += 1;

			}
			//Debug.Log (playersReady);

		}

		if (players.Length >= 2 && playersReady == players.Length) {
			Debug.Log ("All PLayers Ready");
			SceneManager.LoadScene ("ChazScene2");
		}
		else {
			playersReady = 0;
		}
    }
}
