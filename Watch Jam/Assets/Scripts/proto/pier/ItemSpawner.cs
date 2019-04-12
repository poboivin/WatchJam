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
    public bool willSpawnItem = false;
    public bool isSpawned = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isSpawned && willSpawnItem)
        {
            Item = Instantiate(ItemPrefab, SpawnLoc.position, SpawnLoc.rotation);
            isSpawned = true;
            willSpawnItem = false;
        }

        if (Item == null)
        {
            isSpawned = false;
        }

    }

    void Spawn()
    {

    }

}
