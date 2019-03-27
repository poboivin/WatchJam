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

    private void OnTriggerEnter2D( Collider2D collider )
    {
        NewMelee player = gameObject.GetComponentInParent<NewMelee>();
        if( player )
        {
            Debug.LogFormat( "hit {0} with effect", collider.name );
            player.DamagePlayer( collider );
        }
    }
}
