using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script represent a panel to show individual game records in game result UI
/// </summary>
public class GameResultUIScript : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] PlayerRecordUIScript[] playerPanel;
    // Start is called before the first frame update
    void Start()
    {
        mainPanel.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameResultUI()
    {
        mainPanel.SetActive( true );
        var statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager )
        {
            foreach( var panel in playerPanel )
            {
                panel.UpdateStatisticInfo();
            }
        }
    }

    public void ButtonClicked( int buttonId )
    {
        //Debug.Log( "Button clicked : " + buttonId );
        if( buttonId == 0 )
        {
            // close game result UI, and restart the game
            mainPanel.SetActive( false );
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene( scene.name );
        }
    }
}
