using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float remainingTime;

    private bool onTimer = false;
    public bool hitTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        if( Settings.s.gameMode == GameMode.TimeLimit )
            StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if( onTimer )
        {
            remainingTime = Mathf.Max( 0, remainingTime - Time.deltaTime );
            if( remainingTime == 0.0f )
            {
                onTimer = false;
                hitTimer = true;
            }
        }
    }

    public void StartTimer()
    {
        remainingTime = Settings.s.timeLimitInSceonds;
        onTimer = true;
        hitTimer = false;
    }

    public void PauseTimer()
    {
        onTimer = false;
    }

    public void ResumeTimer()
    {
        onTimer = true;
    }

    public void StopTimer()
    {
        onTimer = false;
        remainingTime = 0.0f;
    }
}
