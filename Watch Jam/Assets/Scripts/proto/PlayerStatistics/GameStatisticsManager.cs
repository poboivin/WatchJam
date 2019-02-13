using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class manages all the player statistic information from each player
/// a player information is updated when the new player is spawned.
/// </summary>
public class GameStatisticsManager : MonoBehaviour {

    PlayerStatisticInfo[] playersInfo;

    void Awake()
    {
        playersInfo = new PlayerStatisticInfo[( int )PierInputManager.PlayerNumber.PC];
        for( var i = 0; i < ( int )PierInputManager.PlayerNumber.PC; i++ )
        {
            playersInfo[i].PlayerId = i;
        }
    }

    void Clear()
    {
        for( var i = 0; i < ( int )PierInputManager.PlayerNumber.PC; i++ )
        {
            // TODO : check only play this seesion? or whole session? 
            // what if a player plays intermittently
            //playersInfo[i].IsPlayed = false;

            playersInfo[i].IsWonGame = false;
            playersInfo[i].TotalShots = 0;
            playersInfo[i].TotalHits = 0;
        }
    }

    public void PlayerStarted( int playerId )
    {
        playersInfo[playerId].Started();
    }

    public void PlayerDied( int playerId )
    {
        playersInfo[playerId].Died();

        var hitter = playersInfo[playerId].PlayerIdLastHitter;
        playersInfo[hitter].PlayerKills += 1;
    }

    public void PlayerWon( int playerId )
    {
        playersInfo[playerId].WonGame();
    }

    public void RecordFire( int playerId )
    {
        playersInfo[playerId].TotalShots += 1;
    }

    public void RecordHitTarget( int playerId )
    {
        playersInfo[playerId].TotalHits += 1;
    }

    public void RecordHitTarget( int shooter, int casualty )
    {
        playersInfo[shooter].TotalHits += 1;
        playersInfo[casualty].PlayerIdLastHitter = shooter;
    }

    public PlayerStatisticInfo GetPlayerStatisticInfo( int playerId )
    {
        return playersInfo[playerId];
    }
}
