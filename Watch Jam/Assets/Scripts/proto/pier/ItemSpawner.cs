using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform Item;
    public Transform SpawnLoc;
    public Transform ItemPrefab;
    public float SpawnDelay = 5f;
    private float spawnTimer = 0;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if( Item == null)
        {
            spawnTimer += Time.deltaTime;

            if(spawnTimer >= SpawnDelay)
            {
                spawnTimer = 0;
                Item = Instantiate(ItemPrefab, SpawnLoc.position, SpawnLoc.rotation);
            }
        }
	}
}
