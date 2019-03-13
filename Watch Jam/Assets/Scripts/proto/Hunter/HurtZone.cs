using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtZone : MonoBehaviour
{

    [SerializeField]
    float damage;
    public SpriteRenderer CompanionVisual;

        Vector2 RightForce = new Vector2(3000,1000);
        Vector2 LeftForce = new Vector2(-3000, -1000);


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
            collision.gameObject.GetComponent<LifeSpan>().SubstactLife(damage);
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(LeftForce);
            }

            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(RightForce);
            }
        }
    }


}