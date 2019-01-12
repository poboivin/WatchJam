﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchCounter : MonoBehaviour
{
    public AudioClip[] killshots;
    public AudioSource source;
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
        if(_Instance!= null && _Instance.killshots != null  && _Instance.source != null)
        {
            int random = Random.Range(0, _Instance.killshots.Length);
            _Instance.source.clip = _Instance.killshots[random];
            _Instance.source.Play();
            Debug.Log(random);
        }
      
    }
    void Awake ()
    {
        _Instance = this;
      
      
	}
	public void GameOver()
    {
        Debug.Log("game over");
        SceneManager.LoadScene(1);
    }
	// Update is called once per frame
	void Update ()
    {
		
	}
}