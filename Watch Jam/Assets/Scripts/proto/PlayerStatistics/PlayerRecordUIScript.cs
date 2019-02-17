using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages a game result UI screen.
/// </summary>
public class PlayerRecordUIScript : MonoBehaviour
{
    [SerializeField] PierInputManager.PlayerNumber playerId;
    [SerializeField] Text playerName;
    [SerializeField] Text playerNumKills;
    [SerializeField] Text playerNumDeaths;
    [SerializeField] Text playerNumShots;
    [SerializeField] Text playerNumHits;
    [SerializeField] Text playerShotAccuracy;
    [SerializeField] Text playerIsWon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStatisticInfo()
    {
        var statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager == null )
            return;

        PlayerStatisticInfo statisticInfo = statisticsManager.GetPlayerStatisticInfo( ( int )playerId );
        gameObject.SetActive( statisticInfo.IsPlayed );
        if( statisticInfo.IsPlayed )
        {
            playerName.text = "P" + (statisticInfo.PlayerId+1).ToString();
            playerNumKills.text = "Kills : " + statisticInfo.PlayerKills;
            playerNumDeaths.text = "Deaths : " + statisticInfo.NumOfDeaths;
            playerNumShots.text = "Total Shots : " + statisticInfo.TotalShots;
            playerNumHits.text = "Total Hits : " + statisticInfo.TotalHits;
            int accuracy = 0;
            if( statisticInfo.TotalShots > 0 )
            {
                accuracy = statisticInfo.TotalHits * 100 / statisticInfo.TotalShots;
            }
            playerShotAccuracy.text = "Shot Accuracy : " + accuracy + "%";
            playerIsWon.text = statisticInfo.IsWonGame ? "Win!!!" : string.Empty;
        }
    }


}
