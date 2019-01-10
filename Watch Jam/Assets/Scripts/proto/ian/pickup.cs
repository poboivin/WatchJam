using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public enum pickUpType { health, grenade}
    public pickUpType type;
    public Rigidbody2D Grenade;              // Prefab of the rocket.

    // Use this for initialization
    Vector3 start;
    Vector3 end;
	void Start ()
    {
        start = transform.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = LerpUtility.SineWave(Time.time, start, Vector3.up,0.2f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (type == pickUpType.grenade)
            {
                Ammo player = collision.GetComponent<Ammo>();
                if(player != null && player.enabled == true)
                {
                    player.Grenade = Grenade;
                }
            }
            Destroy(gameObject);
            //play pickup sound
        }

    }
}
