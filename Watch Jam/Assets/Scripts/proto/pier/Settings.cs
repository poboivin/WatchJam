using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        noLimitsToggle.isOn = s.noLimits;
        lifeDecayToggle.isOn = s.lifeDecay;
        //gunKockBackSlider.value = s.gunKnockBack;
        lifeStealToggle.isOn = s.lifeSteal;
        timeStopStoreToggle.isOn = s.timeStopStore;
        rewindKillVelocityToggle.isOn = s.rewindKillVelocity;
        timeStopKillVelocityToggle.isOn = s.timeStopKillVelocity;
        timeStopAmmoRegenToggle.isOn = s.timeStopAmmoRegen;
        rewindAmmoRegenToggle.isOn = s.rewindAmmoRegen;
        passiveAmmoRegenToggle.isOn = s.passiveAmmoRegen;
    }
    public Toggle noLimitsToggle;
	public void noLimits(bool val)
    {
        s.noLimits = val;
    }
    public Toggle lifeDecayToggle;

    public void lifeDecay(bool val)
    {
        s.lifeDecay = val;
    }
    public Slider gunKockBackSlider;
    public void gunKockBack(float val)
    {
        Debug.Log("yoo");
        s.gunKnockBack = val;
    }
    public Toggle lifeStealToggle;

    public void lifeSteal(bool val)
    {
        s.lifeSteal = val;
    }
    public Toggle timeStopStoreToggle;

    public void timeStopStore(bool val)
    {
        s.timeStopStore = val;
    }
    public Toggle rewindKillVelocityToggle;

    public void rewindKillVelocity(bool val)
    {
        s.rewindKillVelocity = val;
    }
    public Toggle timeStopKillVelocityToggle;

    public void timeStopKillVelocity(bool val)
    {
        s.timeStopKillVelocity = val;
    }
    public Toggle timeStopAmmoRegenToggle;

    public void timeStopAmmoRegen(bool val)
    {
        s.timeStopAmmoRegen = val;
    }
    public Toggle rewindAmmoRegenToggle;

    public void rewindAmmoRegen(bool val)
    {
        s.rewindAmmoRegen = val;
    }
    public Toggle passiveAmmoRegenToggle;

    public void passiveAmmoRegen(bool val)
    {
        s.passiveAmmoRegen = val;
    }
    public Toggle rewindInvincibilityToggle;

    public void rewindInvincibility(bool val)
    {
        s.rewindInvincibility = val;
    }
  
}
