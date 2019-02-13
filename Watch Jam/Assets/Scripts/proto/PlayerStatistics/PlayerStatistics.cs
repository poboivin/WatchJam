using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerStatisticInfo
{
    public int PlayerId;
    public bool IsPlayed;
    public int PlayerKills;
    public int NumOfDeaths;
    public int TotalShots;
    public int TotalHits;
    // to track who kills me
    public int PlayerIdLastHitter;
    public bool IsWonGame;

    public void Started()
    {
        IsPlayed = true;
    }

    public void WonGame()
    {
        IsWonGame = true;
    }

    public void Died()
    {
        NumOfDeaths += 1;
        IsWonGame = false;
    }
}

/// <summary>
/// Each Hero should hold a PlayerStatistics 
/// and updating actual statistics information like number of shots are managed by this class.
/// </summary>
public class PlayerStatistics : MonoBehaviour
{
    int playerId;
    GameStatisticsManager statisticsManager;

    // Start is called before the first frame update
    void Start()
    {
        playerId = ( int )PierInputManager.PlayerNumber.PC;
        if( GetComponent<PierInputManager>() != null )
        {
            playerId = ( int )( GetComponent<PierInputManager>() as PierInputManager ).playerNumber;
        }

        statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager )
        {
            statisticsManager.PlayerStarted( playerId );
        }
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    public void RecordFire()
    {
        if( statisticsManager )
        {
            statisticsManager.RecordFire( playerId );
        }
    }

    public void RecordHitTarget()
    {
        if( statisticsManager )
        {
            statisticsManager.RecordHitTarget( playerId );
        }
    }

    public void RecordHitTarget( int casualty )
    {
        if( statisticsManager )
        {
            statisticsManager.RecordHitTarget( playerId, casualty );
        }
    }
}
