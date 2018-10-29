using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchCounter : MonoBehaviour
{
    public static List<TimeController> players;
    public static MatchCounter _Instance;
	// Use this for initialization
    public static void Register(TimeController player)
    {
        if(players == null)
        {
            players = new List<TimeController>();
        }
        players.Add(player);
    }
    public static void Remove(TimeController player)
    {
        players.Remove(player);
        if (players.Count == 1)
        {
            _Instance.GameOver();
        }
        Debug.Log(players.Count);
    }
    void Awake ()
    {
        _Instance = this;
      
      
	}
	public void GameOver()
    {
        Debug.Log("game over");
        SceneManager.LoadScene(0);
    }
	// Update is called once per frame
	void Update ()
    {
		
	}
}
