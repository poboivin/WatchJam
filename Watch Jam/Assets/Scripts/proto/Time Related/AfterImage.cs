using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private TimeController myTimeController;
    float delay = 0.1f;
    float timer;
    bool spawn;
    public SpriteRenderer[] sprites;
    public LineRenderer myMineRenderer;

    void Start()
    {
        myTimeController = GetComponent<TimeController>();
        myMineRenderer = GetComponent<LineRenderer>();
        timer = delay;
    }

    void SpawnTrail()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            GameObject trailPart = new GameObject();
            SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
            trailPartRenderer.sprite = sprite.sprite;
            trailPartRenderer.sortingLayerID = sprite.sortingLayerID;
            trailPart.transform.position = sprite.transform.position;
            trailPart.transform.localScale = sprite.transform.lossyScale;

            Color color = trailPartRenderer.color;
            color.a -= 0.2f; // replace 0.5f with needed alpha decrement
            trailPartRenderer.color = color;
            Destroy(trailPart, 0.5f); // replace 0.5f with needed lifeTime

        }


    }
    public void DrawLine()
    {
        if(myMineRenderer.enabled == false)
        {
            myMineRenderer.enabled = true;
        }
        myMineRenderer.positionCount = myTimeController.myTimeBody.pointsInTime.Count + 1;
        Vector3 start = transform.position;
        myMineRenderer.SetPosition(0, transform.position);
        for (int i = 0; i < myTimeController.myTimeBody.pointsInTime.Count; i++)
        {
            myMineRenderer.SetPosition(i+1, myTimeController.myTimeBody.pointsInTime[i].Position);

        }

           
    }
    // Use this for initialization
    public void Update()
    {
        if (spawn)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = delay;
                SpawnTrail();
            }
        }
        if (myTimeController.isRewinding)
        {
         
            if (spawn == false)
            {
                timer = 0;
            }
            spawn = true;
        }
        else
        {
            myMineRenderer.enabled = false; 
            spawn = false;

        }
    }
}
