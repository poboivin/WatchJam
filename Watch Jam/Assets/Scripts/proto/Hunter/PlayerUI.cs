using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    
    public Image HealthBar;
    public LifeSpan Health;
    public Gun Ammo;
    public TimeController PlayerTimeController;
    public Sprite[] BarColors;
    public PierInputManager.PlayerNumber PlayerId;

    [SerializeField]
    Image[] KunaiSprites;

    public float KunaiAmountLeft=3f;
    public float currentAmmoPercent;
    
    

    public void dummySetup()
    {
        StartCoroutine(Setup());
    }
    public IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();
        
        var players =  FindObjectsOfType<PierInputManager>();

        foreach(PierInputManager p in players)
        {
            if (p.playerNumber == PlayerId)
            {
               
                var TempHealth = p.GetComponent<LifeSpan>();
                if(TempHealth != null)
                {
                    linkHealth(TempHealth) ;
                }

                var tempAmmo = p.GetComponentInChildren<Gun>();
                if (tempAmmo != null)
                {
                    LinkAmmo(tempAmmo);
                }

                var tempTC = p.GetComponent<TimeController>();

                if(tempTC!= null)
                {
                    LinkTimeController(tempTC);
                }
                
            }


            
        }
        yield return null;
    }



    public void LinkAmmo(Gun G)
    {
        Ammo = G;
        G.OnAmmoUpdate.AddListener(updateAmmoUI);
    }
    public void LinkTimeController(TimeController TC)
    {
        PlayerTimeController = TC;
    }


    public void linkHealth(LifeSpan h)
    {
        Health = h;
       
        
        h.OnLifeUpdate.AddListener(UpdateUI);
        UpdateUI();

    }

  
    private void UpdateUI()
    {
        if( Health != null )
        {
            float Percentage = Health.GetLife() / Settings.s.totalLife;
            HealthBar.rectTransform.anchoredPosition = Vector3.Lerp( new Vector3( -194, -79.9f, 0 ), new Vector3( -2.3f, 5.6f, 0 ), Percentage );

            if( Percentage > .67 )
            {
                HealthBar.sprite = BarColors[0];
            }

            else if( Percentage > .34 )
            {
                HealthBar.sprite = BarColors[1];
            }

            else if( Percentage < .34 )
            {
                HealthBar.sprite = BarColors[2];
            }
        }
    }



    private void updateAmmoUI()
    {

       
        
    }

    // Start is called before the first frame update
    void Start()
    {
        dummySetup();
    }

    // Update is called once per frame
    void Update()
    {
        if( Ammo != null )
        {
            KunaiAmountLeft = ( float )Ammo.GetcurrentAmmo() / 3; //Will remove hardcoding

            if( KunaiAmountLeft > .75 )
            {
                for( int i = 0; i < 3; i++ )
                {
                    KunaiSprites[i].fillAmount = 1;
                }
            }
            else if( KunaiAmountLeft > .45 )
            {
                for( int i = 0; i < 3; i++ )
                {
                    if( i > 1 )
                    {
                        KunaiSprites[i].fillAmount = 0;
                    }
                    else
                    {
                        KunaiSprites[i].fillAmount = 1;
                    }

                }
            }
            else if( KunaiAmountLeft > 0 )
            {
                for( int i = 0; i < 3; i++ )
                {
                    if( i > 0 )
                    {
                        KunaiSprites[i].fillAmount = 0;
                    }
                    else
                    {
                        KunaiSprites[i].fillAmount = 1;
                    }

                }
            }
            else
            {
                //Debug.Log(KunaiSprites.Length,this.gameObject);

                for( int i = 0; i < 3; i++ )
                {
                    KunaiSprites[i].fillAmount = 0;
                }
            }
        }
    }
}
