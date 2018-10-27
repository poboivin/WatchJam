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

    public float TimeFactor = 0.25f;
    // Use this for initialization
    public void Update()
    {
        if (Lerp)
        {
            currentLerpTime += Time.deltaTime;

            float perc = LerpUtility.Lerp(currentLerpTime, lerpTime, lerpMode);

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, perc);
            if (currentLerpTime > lerpTime)
            {

                Lerp = false;
                currentLerpTime = 0;
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
        //     Explode();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        TimeController timeController = col.GetComponent<TimeController>();
        if (timeController != null)
        {
            timeController.myRigidbody2D.velocity /= TimeFactor;
            timeController.SetTimeScale(1);
        }
        //     Explode();
    }
}
