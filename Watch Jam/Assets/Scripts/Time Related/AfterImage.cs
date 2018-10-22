using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour {
    float delay = 0.2f;
    float timer;
    bool spawn;
    public SpriteRenderer sprite;
    void Start()
    {
        timer = delay;
       // InvokeRepeating("SpawnTrail", 0, 0.2f); // replace 0.2f with needed repeatRate
    }

    void SpawnTrail()
    {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.sprite = sprite.sprite;
        trailPart.transform.position = transform.position;
        trailPart.transform.localScale = transform.localScale;
        Destroy(trailPart, 0.5f); // replace 0.5f with needed lifeTime

        StartCoroutine("FadeTrailPart", trailPartRenderer);
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 0.5f; // replace 0.5f with needed alpha decrement
        trailPartRenderer.color = color;

        yield return new WaitForEndOfFrame();
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
        if (Input.GetKeyDown(KeyCode.E))
        {
                spawn = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            spawn = false;

        }
    }
}
