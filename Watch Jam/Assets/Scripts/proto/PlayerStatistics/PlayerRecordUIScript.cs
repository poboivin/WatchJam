using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages a game result UI screen.
/// </summary>
public class PlayerRecordUIScript : MonoBehaviour
{
    public PierInputManager.PlayerNumber playerId;
    public Sprite largeSprite;
    public Sprite smallSprite;
    [SerializeField]
    Image largeImage;
    [SerializeField]
    Image smallImage;
    [SerializeField] Text numKillsInLarge;
    [SerializeField] Text numDeathsInLarge;
    [SerializeField] Text numKillsInSmall;
    [SerializeField] Text numDeathsInSmall;

    // Start is called before the first frame update
    void Start()
    {
        if( largeImage != null && largeSprite != null )
        {
            largeImage.sprite = largeSprite;
        }
        if( smallImage != null && smallSprite != null )
        {
            smallImage.sprite = smallSprite;
        }
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
            if( numKillsInLarge )
                numKillsInLarge.text = statisticInfo.PlayerKills.ToString();
            if( numKillsInSmall )
                numKillsInSmall.text = statisticInfo.PlayerKills.ToString();
            if( numDeathsInLarge )
                numDeathsInLarge.text = statisticInfo.NumOfDeaths.ToString();
            if( numDeathsInSmall )
                numDeathsInSmall.text = statisticInfo.NumOfDeaths.ToString();
           /* if( largeImage )
                largeImage.gameObject.SetActive( statisticInfo.IsWonGame );
            if( smallImage )
                smallImage.gameObject.SetActive( !statisticInfo.IsWonGame );*/
        }
    }


}
