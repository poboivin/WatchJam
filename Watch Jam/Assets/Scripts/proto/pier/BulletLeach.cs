using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLeach : MonoBehaviour
{
    public LifeSpan myOwner;

    public float damageMultiplier = 1.0f;
	public float knockbackMultiplier = 1.0f;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        LifeSpan otherLife = other.gameObject.GetComponent<LifeSpan>();
        if(otherLife != null && otherLife != myOwner)
        {
            Vector3 dir = other.transform.position - this.transform.position;

			other.GetComponent<TimeController>().AddForce(this.GetComponent<Rigidbody2D>().velocity.normalized * knockbackMultiplier * Settings.s.bulletKnockBack);
            Debug.Log(new Vector2(dir.x, dir.y).normalized);
            Debug.Log(new Vector2(0, dir.y).normalized );
            Debug.Log(new Vector2(dir.x, 0).normalized );


            float stealAmount = otherLife.SubstactLife(Settings.s.bulletDamage * damageMultiplier );
            if(Settings.s.lifeSteal == true)
            {
                myOwner.AddLife(stealAmount);
            }         
        }
    }
}
