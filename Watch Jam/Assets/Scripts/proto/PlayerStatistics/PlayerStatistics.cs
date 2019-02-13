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

    public void Fire()
    {
        TotalShots += 1;
    }

    public void HitTarget()
    {
        TotalHits += 1;
    }

    public void KilledPlayer()
    {
        PlayerKills += 1;
    }
}

/// <summary>
/// Each Hero should hold a PlayerStatistics 
/// and updating actual statistics information like number of shots are managed by this class.
/// </summary>
public class PlayerStatistics : MonoBehaviour
{
    PierInputManager.PlayerNumber playerId;
    GameStatisticsManager statisticsManager;

    // Start is called before the first frame update
    void Start()
    {
        playerId = PierInputManager.PlayerNumber.PC;
        if( GetComponent<PierInputManager>() != null )
        {
            playerId = ( GetComponent<PierInputManager>() as PierInputManager ).playerNumber;
        }

        statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager )
        {
            statisticsManager.PlayerStarted( playerId );
        }
    }
    public void AddFire()
    {
        if( statisticsManager )
        {
            statisticsManager.PlayerFire( playerId );
        }
    }
}
