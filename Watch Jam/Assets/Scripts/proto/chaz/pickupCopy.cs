using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupCopy : MonoBehaviour
{
	public enum pickUpType { health, grenade, chain, freeze}
	public pickUpType type;
	public Rigidbody2D Grenade;              // Prefab of the rocket.
	public Rigidbody2D Chain;  
	public Rigidbody2D Freeze;  
	private GunCopy2 gun;

	// Use this for initialization
	Vector3 start;
	Vector3 end;
	void Start ()
	{
		start = transform.position;
		gun = GetComponent<GunCopy2> ();

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
			Ammo player = collision.GetComponent<Ammo> ();
			if (type == pickUpType.grenade) 
			{

				if (player != null && player.enabled == true) {
					player.Grenade = Grenade;
				}
			} 
			else if (type == pickUpType.chain) 
			{
				if (player != null && player.enabled == true) {
					player.Grenade = Chain;
				}
			}
			else if (type == pickUpType.freeze) 
			{
				if (player != null && player.enabled == true) {
					player.Grenade = Freeze;
					Debug.Log ("BIG");
				}
			}
			Destroy(gameObject);
			//play pickup sound
		}

	}
}
