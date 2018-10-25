using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchCounter : MonoBehaviour
{
    public  List<TimeController> players;
    public static MatchCounter _Instance;
	// Use this for initialization
    public static void Register(TimeController player)
    {
        _Instance.players.Add(player);
    }
    public static void Remove(TimeController player)
    {
        _Instance.players.Remove(player);
        if (_Instance.players.Count == 1)
        {
            _Instance.GameOver();
        }
        Debug.Log(_Instance.players.Count);
    }
    void Awake ()
    {
        _Instance = this;
        players = new List<TimeController>();
        foreach(TimeController t in FindObjectsOfType<TimeController>())
        {
            players.Add(t);
        }
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
