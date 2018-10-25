﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GlobalSettings mainSettings;
    public static GlobalSettings s;
    public static Settings _Instance;
	// Use this for initialization
	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        if (Settings._Instance == null)
        {
            Settings._Instance = this;
        }
        else
        {
            Debug.LogWarning("A previously awakened Settings MonoBehaviour exists!", gameObject);
            Destroy(this.gameObject);
        }
        if (Settings.s == null)
        {
            Settings.s = mainSettings;
        }
    }
	public void  Nolimits(bool val)
    {
        s.noLimits = val;
    }
    public void LifeDecay(bool val)
    {
        s.lifeDecay = val;
    }
    public void KnockBack(float val)
    {
        s.gunKockBack = val;
    }
    public void LifeSteal(bool val)
    {
        s.lifeSteal = val;
    }
    public void TimeStopStore(bool val)
    {
        s.timeStopStore = val;
    }
    public void RewindKillVelocity(bool val)
    {
        s.rewindKillVelocity = val;
    }
    public void StopTimeKillVelocity(bool val)
    {
        s.timeStopKillVelocity = val;
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
}