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
    [SerializeField] PlayerRecordUIScript[] playerPanel;
    [SerializeField] GameObject gameInfoMessage;
    TMPro.TextMeshProUGUI reloadMessage;
    public float restartTime = 10.0f;
    private float restartTimer = 0.0f;
     
    PierInputManager[] inputManagers;
    // Start is called before the first frame update
    void Start()
    {
        if( gameInfoMessage )
        {
            reloadMessage = gameInfoMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            ShowRemainingTime( restartTime );
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if( inputManagers != null )
        //{
        //    foreach( var input in inputManagers )
        //    {
        //        // TODO : shoud this button take out to public variable?
        //        if( input.GetButton( PierInputManager.ButtonName.Fire1 ) )
        //        {
        //            ButtonClicked( 0 );
        //            break;
        //        }
        //    }
        //}
    }

    public void ShowGameResultUI()
    {
        var statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager )
        {
            foreach( var panel in playerPanel )
            {
                panel.UpdateStatisticInfo();
            }
        }
        EnableUserInput( false );
        
        StartCoroutine( LoadNextScene() );
    }

    IEnumerator LoadNextScene()
    {
        restartTimer = restartTime;
        while( restartTimer > 0 )
        {
            ShowRemainingTime( restartTimer );
            yield return new WaitForSeconds( 1.0f );
            restartTimer -= 1.0f;
        }

        EnableUserInput( true );

        // close game result UI, and restart the game
        Scene scene = SceneManager.GetActiveScene();
        if( SceneManager.sceneCountInBuildSettings > 0 )
        {
            Debug.LogFormat( "Scene total = {0}, current = {1}, name = {2}", SceneManager.sceneCountInBuildSettings, scene.buildIndex, scene.name );
            int nextSceneIndex = ( scene.buildIndex + 1 ) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene( nextSceneIndex );
        }
        else
        {
            // just reloading
            SceneManager.LoadScene( scene.name );
        }

    }

    void ShowRemainingTime( float remainingTime )
    {
        if( reloadMessage )
        {
            int time = ( int )remainingTime;
            reloadMessage.text = string.Format( "Game will restart in {0} second{1}", time, time > 1 ? "s" : "" );
        }
    }

    public void ButtonClicked( int buttonId )
    {
        //Debug.Log( "Button clicked : " + buttonId );
        if( buttonId == 0 )
        {
            EnableUserInput( true );

            // close game result UI, and restart the game
            Scene scene = SceneManager.GetActiveScene();
            
            if( SceneManager.sceneCountInBuildSettings > 0 )
            {
                Debug.LogFormat( "Scene total = {0}, current = {1}, name = {2}", SceneManager.sceneCountInBuildSettings, scene.buildIndex, scene.name );
                int nextSceneIndex = ( scene.buildIndex + 1 ) % SceneManager.sceneCountInBuildSettings;
                SceneManager.LoadScene( nextSceneIndex );
            }
            else
            {
                // just reloading
                SceneManager.LoadScene( scene.name );
            }
            
        }
    }

    public void EnableUserInput( bool enable )
    {
        inputManagers = FindObjectsOfType<PierInputManager>();
        if( inputManagers != null )
        {
            foreach( var input in inputManagers )
            {
                if( enable )
                    input.EnableButtonsOnPopup();
                else
                    input.DisableButtonsOnPopup();
            }
        }

    }
}
