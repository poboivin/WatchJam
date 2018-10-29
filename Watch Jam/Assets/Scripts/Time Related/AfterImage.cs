using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private TimeController myTimeController;
    float delay = 0.2f;
    float timer;
    bool spawn;
    public SpriteRenderer[] sprites;


    void Start()
    {
        myTimeController = GetComponent<TimeController>();
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
            color.a -= 0.5f; // replace 0.5f with needed alpha decrement
            trailPartRenderer.color = color;
            Destroy(trailPart, 0.5f); // replace 0.5f with needed lifeTime

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
                spawn = true;
        }
        else
        {
            spawn = false;

        }
    }
}
