using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private TimeController myTimeController;
	private TimeRewindController myRewindController;
    float delay = 0.1f;
    float timer;
    bool spawn;
    public SpriteRenderer[] sprites;
    public LineRenderer myMineRenderer;
	[HideInInspector]
	public bool isSpawned = false;

	GameObject ghostImage;
	public bool showAfterImage = true;
	public bool showTrail = true;
    Material material;
    void Start()
    {
        myTimeController = GetComponent<TimeController>();
        myMineRenderer = GetComponent<LineRenderer>();
		myRewindController = GetComponent<TimeRewindController> ();
        material = GetComponentInChildren<Animator>().GetComponent<SpriteRenderer>().material;
        timer = delay;

    }

    void SpawnTrail()
    {
		if (showAfterImage) 
		{
			foreach (SpriteRenderer sprite in sprites) {
				GameObject trailPart = new GameObject ();
				SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer> ();
				trailPartRenderer.sprite = sprite.sprite;
				trailPartRenderer.sortingLayerID = sprite.sortingLayerID;
				trailPart.transform.position = sprite.transform.position;
				trailPart.transform.localScale = sprite.transform.lossyScale;

				Color color = trailPartRenderer.color;
				color.a -= 0.2f; // replace 0.5f with needed alpha decrement
				trailPartRenderer.color = color;
				Destroy (trailPart, 0.5f); // replace 0.5f with needed lifeTime

			}
		}


    }
    public void DrawLine()
    {
		if (showTrail) {
			if (myMineRenderer.enabled == false) {
				myMineRenderer.enabled = true;
			}
			myMineRenderer.positionCount = myTimeController.myTimeBody.pointsInTime.Count + 1;
			Vector3 start = transform.position;
			myMineRenderer.SetPosition (0, transform.position);
			for (int i = 0; i < myTimeController.myTimeBody.pointsInTime.Count; i++)
            {
				myMineRenderer.SetPosition (i + 1, myTimeController.myTimeBody.pointsInTime [i].Position);
			}
			if (myRewindController.useNewRewind)
            {
				foreach (SpriteRenderer sprite in sprites)
                {
					if (!isSpawned)
                    {
						ghostImage = new GameObject ();
					}
                    else
						ghostImage.SetActive (true);

					isSpawned = true;
					if (!ghostImage.GetComponent (typeof(SpriteRenderer)))
                    {
						SpriteRenderer ghostRenderer = ghostImage.AddComponent<SpriteRenderer> ();

                       // ghostRenderer.color = new Color(ghostRenderer.color.r, ghostRenderer.color.g, ghostRenderer.color.b, .65f);

                        ghostRenderer.sprite = sprite.sprite;
                        ghostRenderer.material = material;
                 //       ghostRenderer.material.
                        ghostRenderer.material.SetColor("_Tint", new Color(0.0f, 0.0f, 0.0f, .45f));

                        ghostRenderer.sortingLayerID = sprite.sortingLayerID;
					}
					ghostImage.transform.position = myTimeController.myTimeBody.pointsInTime [myTimeController.myTimeBody.pointsInTime.Count - 1].Position;
					ghostImage.transform.localScale = sprite.transform.lossyScale;

				}
			}

		} else
			DisableGhost ();
    }

	public void DisableGhost()
    {
		myMineRenderer.enabled = false;
        if(ghostImage != null)
        {
            ghostImage.SetActive(false);

        }
    }
    public void OnDisable()
    {
        DisableGhost();

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
			if (!myRewindController.useNewRewind) 
			{
				myMineRenderer.enabled = false; 
				spawn = false;
			}

        }
    }
}
