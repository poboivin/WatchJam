using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class manages all the player statistic information from each player
/// a player information is updated when the new player is spawned.
/// </summary>
public class GameStatisticsManager : MonoBehaviour {

    PlayerStatisticInfo[] playersInfo = new PlayerStatisticInfo[(int)PierInputManager.PlayerNumber.PC];

    void Clear()
    {
        playersInfo = new PlayerStatisticInfo[( int )PierInputManager.PlayerNumber.PC];
    }

    public void PlayerStarted( PierInputManager.PlayerNumber playerId )
    {
        playersInfo[( int )playerId].Started();
    }

    public void PlayerDied( PierInputManager.PlayerNumber playerId )
    {
        playersInfo[( int )playerId].Died();
    }

    public void PlayerWon( PierInputManager.PlayerNumber playerId )
    {
        playersInfo[( int )playerId].WonGame();
    }

    public void PlayerFire(PierInputManager.PlayerNumber playerId )
    {
        playersInfo[( int )playerId].Fire();
    }

    public void PlayerHit( PierInputManager.PlayerNumber playerId )
    {
        playersInfo[( int )playerId].HitTarget();
    }

    public void PlayerKill( PierInputManager.PlayerNumber playerId )
    {
        playersInfo[( int )playerId].KilledPlayer();
    }

    public PlayerStatisticInfo GetPlayerStatisticInfo( PierInputManager.PlayerNumber playerId )
    {
        return playersInfo[( int )playerId];
    }

    public void SetPlayerStatisticInfo( int index, PlayerStatisticInfo playerInfo )
    {
        if( index < (int)PierInputManager.PlayerNumber.PC )
        {
            playersInfo[index] = playerInfo;
        }
    }
}
