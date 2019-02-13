using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerStatisticInfo
{
    public PierInputManager.PlayerNumber PlayerId;
    public bool IsPlayed;
    public int PlayerKills;
    public int NumOfDeaths;
    public int TotalShots;
    public int TotalHits;
    public float ShotAccuracy;
    public bool IsWonGame;

    public void Died()
    {
        NumOfDeaths += 1;
        IsWonGame = false;
        if( TotalHits > 0 )
            ShotAccuracy = TotalShots / TotalHits;
    }
}

/// <summary>
/// Each Hero should hold a PlayerStatistics 
/// and updating actual statistics information like number of shots are managed by this class.
/// </summary>
public class PlayerStatistics : MonoBehaviour
{

    PlayerStatisticInfo playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        PierInputManager.PlayerNumber playerId = PierInputManager.PlayerNumber.PC;
        if( GetComponent<PierInputManager>() != null )
        {
            playerId = ( GetComponent<PierInputManager>() as PierInputManager ).playerNumber;
        }

        playerInfo = new PlayerStatisticInfo
        {
            PlayerId = playerId,
            IsPlayed = true,
            PlayerKills = 0,
            NumOfDeaths = 0,
            TotalShots = 0,
            TotalHits = 0,
            ShotAccuracy = 0,
            IsWonGame = true
        };

        var statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager != null )
            statisticsManager.SetPlayerStatisticInfo( (int)playerId, playerInfo );
    }
}
