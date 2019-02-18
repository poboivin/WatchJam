using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MatchCounter : MonoBehaviour
{
    public AudioClip[] killshots;
    public AudioSource source;
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
    }
    public static void Remove(TimeController player)
    {
        players.Remove(player);

        var targetPlayerNumber = PierInputManager.PlayerNumber.PC;
        var inputManager = player.GetComponent<PierInputManager>() as PierInputManager;
        if( inputManager )
            targetPlayerNumber = inputManager.playerNumber;

        var statisticsManager = FindObjectOfType<GameStatisticsManager>();
        if( statisticsManager && targetPlayerNumber != PierInputManager.PlayerNumber.PC )
        {
            statisticsManager.PlayerDied( ( int )targetPlayerNumber );
        }

        if (players.Count == 1)
        {
            var wonPlayerInputManager = ( players[0].GetComponent<PierInputManager>() as PierInputManager );
            if( statisticsManager && wonPlayerInputManager )
                statisticsManager.PlayerWon( ( int )wonPlayerInputManager.playerNumber );

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
        
        var inputManager = player.GetComponent<PierInputManager>() as PierInputManager;
        if( inputManager )
        {
            var playerNumber = inputManager.playerNumber;
            var spawner = playerSpawners[( int )playerNumber].GetComponent<PlayerSpawn>();
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
        }
		
	}
}
