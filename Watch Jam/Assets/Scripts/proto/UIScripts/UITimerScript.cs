using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimerScript : MonoBehaviour
{
    [SerializeField]
    Text remainingTimeText;

    [SerializeField]
    GameTimer timer;
    // Start is called before the first frame update
    void Start()
    {
        remainingTimeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if( Settings.s.gameMode == GameMode.TimeLimit )
        {
            remainingTimeText.text = string.Format( "{0:00}:{1:00}", ( int )( timer.remainingTime / 60.0f ), ( int )timer.remainingTime % 60 );
        }
    }
}
