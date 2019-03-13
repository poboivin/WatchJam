using UnityEngine;
using System.Collections;

public class RocketPierce : Rocket 
{
	private void Start()
	{
		GetComponentInChildren<Animator>().Play("KunaiLoop");
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.tag == "Player" )
		{
			if(col.gameObject != myOwner.gameObject)
			{
				OnExplode();
				if (Camera.main.GetComponent<CamShake>() != null)
					Camera.main.GetComponent<CamShake>().Shake(0.1f, 0.2f);
				var shooter = myOwner.gameObject.GetComponentInParent<PlayerStatistics>();
				if( shooter != null )
				{
					var casualty = col.gameObject.GetComponentInParent<PlayerStatistics>();
					if( casualty != null )
					{
						shooter.RecordHitTarget( casualty.GetPlayerId() );
					}
					else
					{
						shooter.RecordHitTarget();
					}
				}
			}


		}

		else if (col.gameObject.tag == "destructable")
		{
			OnExplode();
			if (Camera.main.GetComponent<CamShake>() != null)
				Camera.main.GetComponent<CamShake>().Shake(0.1f, 0.2f);
			col.gameObject.GetComponent<destructable>().shatter();
			Destroy(gameObject);
		}

		else if (col.tag == "ground" || col.tag == "Obstacle")
		{
			OnExplode();
			Destroy(gameObject);
		}
		else if (col.tag == "Bullet")
		{
			Rocket otherRocket = col.GetComponent<Rocket>();
			if(otherRocket.myOwner != myOwner)
			{
				OnExplode();
				Destroy(gameObject);
			}

		}

		// Otherwise if the player manages to shoot himself...
		else if (col.gameObject.tag == null)
		{
			OnExplode();
			Destroy(gameObject);
		}
	}
}
