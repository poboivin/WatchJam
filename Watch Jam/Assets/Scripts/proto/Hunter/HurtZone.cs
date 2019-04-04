using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtZone : MonoBehaviour
{

    [SerializeField]
    float damage;

    [SerializeField]
    float HurtRate=3;
    public SpriteRenderer CompanionVisual;

    Vector2 RightForce = new Vector2(3000,1000);
    Vector2 LeftForce = new Vector2(-3000, -1000);

    List<LifeSpan> Players;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<LifeSpan>())
        {
            collision.gameObject.GetComponent<LifeSpan>().SubstactLife(9);
        }
    }



    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<LifeSpan>())
    //    {
    //        collision.gameObject.GetComponent<LifeSpan>().Hurtzone(HurtRate);
    //    }

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<LifeSpan>())
    //    {
    //        collision.gameObject.GetComponent<LifeSpan>().HurtStartTime=Time.time;

    //    }
    //}





}