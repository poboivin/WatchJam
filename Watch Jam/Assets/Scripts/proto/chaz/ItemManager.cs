using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform[] items;
    public float minSpawnWait = 5.0f;
    public float maxSpawnWait = 10.0f;
    public int maxItemsSpawned = 3;

    ItemSpawner[] itemSpawners;
   

    private float spawnDelay;
    private float spawnTimer;
    private int numItemsSpawned = 0;


    void Awake()
    {
        itemSpawners = FindObjectsOfType<ItemSpawner>();
        spawnDelay = Random.Range(minSpawnWait, maxSpawnWait);
        
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (numItemsSpawned == maxItemsSpawned) //Prevents an item from spawning immediately if an item is picked up when at max items spawned
        {
            spawnTimer = 0;
        }

        List<ItemSpawner> itemSpawnersEmpty = new List<ItemSpawner>();
        numItemsSpawned = 0;
        foreach (ItemSpawner itemspawner in itemSpawners)
        {
            if (itemspawner.isSpawned)
            {
                numItemsSpawned += 1;
            }
            else
            {
                itemSpawnersEmpty.Add(itemspawner);
            }
        }


        if ((spawnTimer >= spawnDelay) && (numItemsSpawned < maxItemsSpawned) && itemSpawnersEmpty.Count > 0)  //only spawn when there are less items than we would want
        {
            int randomItem = Random.Range(0, items.Length);                 //pick random item prefab to spawn
            int randomSpawner = Random.Range(0, itemSpawnersEmpty.Count);   //pick random empty spawner to spawn an item
            itemSpawnersEmpty[randomSpawner].ItemPrefab = items[randomItem];    //set empty spawners item to the random
            itemSpawnersEmpty[randomSpawner].willSpawnItem = true;              //tell spawner to spawn new item

            spawnDelay = Random.Range(minSpawnWait, maxSpawnWait);
            spawnTimer = 0;
        }

    }
}
