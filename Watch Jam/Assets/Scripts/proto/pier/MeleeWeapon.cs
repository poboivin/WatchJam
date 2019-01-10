using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [HideInInspector]
    public LifeSpan myOwner;

    void OnTriggerEnter2D(Collider2D col)
    {


        if (col.tag == "Player")
        {
           
            LifeSpan otherLife = col.gameObject.GetComponent<LifeSpan>();
            if (otherLife != null && otherLife != myOwner)
            {
                Vector3 dir = col.transform.position - this.transform.position;

                col.GetComponent<TimeController>().AddForce(new Vector2(dir.x, 0).normalized * Settings.s.bulletKnockBack * 10);
                myOwner.GetComponent<TimeController>().AddForce(new Vector2(-dir.x, 0).normalized * Settings.s.bulletKnockBack * 10);
               // Debug.Log(col.GetComponent<TimeController>().gameObject.name);
                float stealAmount = otherLife.SubstactLife(Settings.s.bulletDamage);
                if (Settings.s.lifeSteal == true)
                {
                    myOwner.AddLife(stealAmount);
                }
            }
        }
        else if (col.gameObject.tag == "destructable")
        {
            
            col.gameObject.GetComponent<destructable>().shatter();
        }
        else if (col.tag == "Bullet")
        {
            Rocket otherRocket = col.GetComponent<Rocket>();
            if (otherRocket.myOwner != myOwner)
            {
                otherRocket.OnExplode();
                Destroy(gameObject);
            }

        }

      
    }
}
