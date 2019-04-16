using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawn : MonoBehaviour
{
    public UnityEvent OnPlayerSpawn;
    public PierInputManager inputManager;
    public PierInputManager.ButtonName SpawnButton;
    public GameObject HeroPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (inputManager.GetButtonDown(SpawnButton))
        {
            OnPlayerSpawn.Invoke();
        }
	}


   public void PlaySpawnAudio()
    {
        GetComponent<AudioSource>().Play();
    }
}
