using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MatchCounter : MonoBehaviour
{
    public AudioClip[] killshots;
    public AudioSource source;
    public GameObject killEffect;
    public static List<TimeController> players;
    public static MatchCounter _Instance;

    private GameTimer timer;

    public GameObject[] playerSpawners;
	// Use this for initialization
    public static void Register(TimeController player)
    {
        if(players == null)
        {
            players = new List<TimeController>();
        }
        players.Add(player);
        if( Settings.s.gameMode == GameMode.TimeLimit && players.Count == 1 )
        {
            _Instance.timer.StartTimer();
        }
    }

    public static void Remove(TimeController player)
    {
        players.Remove(player);

        var statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager && player.PlayerId != PierInputManager.PlayerNumber.PC )
        {
            var killer = statisticsManager.PlayerDied( ( int )player.PlayerId );
            if( _Instance.killEffect )
            {
                foreach( var hitter in players )
                {
                    if( hitter.PlayerId == (PierInputManager.PlayerNumber)killer )
                    {
                        var playerPos = hitter.gameObject.transform.position;
                        playerPos.y += 2;
                        Instantiate( _Instance.killEffect, playerPos, Quaternion.identity );
                        break;
                    }
                }
            }
        }

        if (players.Count == 1)
        {
            if( statisticsManager && players[0].PlayerId != PierInputManager.PlayerNumber.PC )
                statisticsManager.PlayerWon( ( int )players[0].PlayerId );

            if( Settings.s.gameMode == GameMode.Normal )
                _Instance.GameOver();
        }
        if(_Instance!= null && _Instance.killshots != null  && _Instance.source != null)
        {
            int random = Random.Range(0, _Instance.killshots.Length);
            _Instance.source.clip = _Instance.killshots[random];
            _Instance.source.Play();
            Debug.Log(random);
        }

        if( Settings.s.gameMode == GameMode.TimeLimit )
        {
            _Instance.StartCoroutine( _Instance.RespawnCoroutine( player ) );
        }
    }

    IEnumerator RespawnCoroutine( TimeController player )
    {
        yield return new WaitForSeconds( Settings.s.playerRespawnTime );

        if( player.PlayerId != PierInputManager.PlayerNumber.PC )
        {
            var spawner = playerSpawners[( int )player.PlayerId].GetComponent<PlayerSpawn>();
            var newHero = Instantiate( spawner.HeroPrefab, spawner.transform.position, Quaternion.identity );
            Destroy( player.gameObject );
            newHero.gameObject.SetActive( true );
            Register( newHero.GetComponent<TimeController>() );
        }
    }

    void Awake ()
    {
        _Instance = this;
        timer = GetComponent<GameTimer>();
    }

    public void GameOver()
    {
        Debug.Log("game over");

        var gameEndUI = FindObjectOfType<GameResultUIScript>();
        if( gameEndUI )
        {
            gameEndUI.ShowGameResultUI();
        }
        if( players != null )
            players.Clear();
        //SceneManager.LoadScene(1);
    }
	// Update is called once per frame
	void Update ()
    {
        if( Settings.s.gameMode == GameMode.TimeLimit && timer != null && timer.hitTimer )
        {
            var statisticsManager = FindObjectOfType<GameStatisticsManager>();
            if( statisticsManager )
            {
                int maxPlayerKills = 0;
                int maxPlayerKillerId = ( int )PierInputManager.PlayerNumber.PC;
                for( int i = 0; i < ( int )PierInputManager.PlayerNumber.PC; i++ )
                {
                    var statistics = statisticsManager.GetPlayerStatisticInfo( i );
                    if( statistics.PlayerKills > maxPlayerKills )
                    {
                        maxPlayerKills = statistics.PlayerKills;
                        maxPlayerKillerId = i;
                    }
                }

                if( maxPlayerKillerId != ( int )PierInputManager.PlayerNumber.PC )
                {
                    statisticsManager.PlayerWon( maxPlayerKillerId );
                }
            }
            GameOver();
            timer.StopTimer();
        }
		
	}
}
