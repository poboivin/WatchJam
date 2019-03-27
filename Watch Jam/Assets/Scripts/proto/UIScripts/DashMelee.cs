using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMelee : MonoBehaviour
{

    private List<Collider2D> ignored;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        LifeSpan otherPlayer = collision.GetComponent<Collider2D>().gameObject.GetComponent<LifeSpan>();

        if( otherPlayer )
        {
            //Debug.LogFormat( "hit {0} in {1}", otherPlayer.name, currentState.ToString() );

            otherPlayer.SubstactLife( Settings.s.meleeDamage );
            Vector3 dir = otherPlayer.transform.position - gameObject.transform.position;
            otherPlayer.GetComponent<TimeController>().AddForce( gameObject.GetComponentInParent<Rigidbody2D>().velocity.normalized * Settings.s.bulletKnockBack );
            if( ignored == null )
            {
                ignored = new List<Collider2D>();
            }

            ignored.Add( collision.GetComponent<Collider2D>() );
            Physics2D.IgnoreCollision( gameObject.GetComponent<Collider2D>(), collision.GetComponent<Collider2D>(), true );

        }
    }
}
