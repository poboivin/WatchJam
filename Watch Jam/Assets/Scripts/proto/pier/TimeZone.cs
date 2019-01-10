using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class TimeZone : MonoBehaviour
{
    public float lerpTime = 1f;
    float currentLerpTime;
    public bool Lerp = true;
    public LerpUtility.lerpMode lerpMode;
    public float TimeAlive = 10f;
    public float TimeFactor = 0.25f;
    private Vector3 endSize;
    public bool canDie = true;
    // Use this for initialization
    public void Start()
    {
        endSize = transform.localScale;
    }
    public void Update()
    {

        if (Lerp)
        {
            currentLerpTime += Time.deltaTime;

            float perc = LerpUtility.Lerp(currentLerpTime, lerpTime, lerpMode);

            transform.localScale = Vector3.Lerp(Vector3.zero, endSize, perc);
            if (currentLerpTime > lerpTime)
            {

                Lerp = false;
                currentLerpTime = 0;
            }
        }
        else
        {
            TimeAlive -= Time.deltaTime;
            if(TimeAlive <= 0)
            {
                death();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        TimeController timeController = col.GetComponent<TimeController>();
        if(timeController!= null)
        {

            timeController.SetTimeScale(  TimeFactor);
            timeController.myRigidbody2D.velocity *= TimeFactor;
        }

        //delete if come in to contact with other zone
        if (col.GetComponent<TimeZone>() != null)
        {
            death();
        }
        //delete if come into contact with teleporter 
        if(col.GetComponent<teleporter>() != null)
        {
            death();
        }
        //     Explode();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        TimeController timeController = col.GetComponent<TimeController>();
        if (timeController != null)
        {
            timeController.myRigidbody2D.velocity /= TimeFactor;
            timeController.ResetTimeScale();
        }
        //     Explode();
    }
    private void death()
    {
        if (canDie)
        {
            Destroy(this.gameObject);

        }
    }
}
