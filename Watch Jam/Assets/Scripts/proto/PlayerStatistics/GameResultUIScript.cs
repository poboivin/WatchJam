using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public int SceneLoad;

    PierInputManager[] inputManagers;
    // Start is called before the first frame update
    void Start()
    {
        if (gameInfoMessage)
        {
            reloadMessage = gameInfoMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            ShowRemainingTime(restartTime);
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
        if (statisticsManager)
        {
            PierInputManager.PlayerNumber Winner;
            PierInputManager.PlayerNumber NotWinner;

            foreach (var panel in playerPanel)
            {
                PlayerStatisticInfo statisticInfo = statisticsManager.GetPlayerStatisticInfo((int)panel.playerId);
                if (statisticInfo.IsWonGame)
                {
                    if(panel != playerPanel[0])
                    {
                        Winner = panel.playerId;
                        NotWinner = playerPanel[0].playerId;
                        Sprite large = panel.largeSprite;
                        Sprite small = panel.smallSprite;

                        panel.smallSprite = playerPanel[0].smallSprite;
                        panel.largeSprite = playerPanel[0].largeSprite;

                        playerPanel[0].smallSprite = small;
                        playerPanel[0].largeSprite = large;

                        playerPanel[0].playerId = Winner;
                        panel.playerId = NotWinner;

                    }


                }
            }
           // PlayerStatisticInfo statisticInfo    = statisticsManager.GetPlayerStatisticInfo((int)playerPanel[0].playerId);
            

            foreach (var panel in playerPanel)
            {
                panel.UpdateStatisticInfo();
            }
        }
        EnableUserInput(false);

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        restartTimer = restartTime;
        while (restartTimer > 0)
        {
            ShowRemainingTime(restartTimer);
            yield return new WaitForSeconds(1.0f);
            restartTimer -= 1.0f;
        }

        EnableUserInput(true);

        // close game result UI, and restart the game
        Scene scene = SceneManager.GetActiveScene();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }

        else
        {
            SceneManager.LoadScene(0);
        }

    }

    void ShowRemainingTime(float remainingTime)
    {
        if (reloadMessage)
        {
            int time = (int)remainingTime;
            reloadMessage.text = string.Format("Game will restart in {0} second{1}", time, time > 1 ? "s" : "");
        }
    }

    public void ButtonClicked(int buttonId)
    {
        //Debug.Log( "Button clicked : " + buttonId );
        if (buttonId == 0)
        {
            EnableUserInput(true);

            // close game result UI, and restart the game
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(1);
            }

            else
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    public void EnableUserInput(bool enable)
    {
        inputManagers = FindObjectsOfType<PierInputManager>();
        if (inputManagers != null)
        {
            foreach (var input in inputManagers)
            {
                if (enable)
                    input.EnableButtonsOnPopup();
                else
                    input.DisableButtonsOnPopup();
            }
        }

    }
}
