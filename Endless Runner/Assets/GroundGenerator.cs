using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public List<GameObject> groundPrefabs;      // List of ground prefabs to choose from
    public int poolSize = 4;                    // Number of ground objects in the object pool
    public float spawnDistance;                 // Distance between each ground spawn
    public Transform player;                    // Reference to the player object

    private List<GameObject> groundPool;        // Object pool for the ground objects
    private float nextSpawnZ;                   // Z position for the next ground spawn

    private void Start()
    {
        GameObject initialground = Instantiate(groundPrefabs[0], transform);
        // Initialize the object pool
        groundPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ground = Instantiate(GetRandomGroundPrefab(), transform);
            ground.SetActive(false);
            groundPool.Add(ground);
        }

        // Set the initial spawn position
        nextSpawnZ = 0f;

        // Start spawning the ground objects
        SpawnGround();
    }

    private void Update()
    {
        // Check if the player has moved past the first ground object
        if (player.position.z > groundPool[0].transform.position.z + spawnDistance)
        {
            // Reposition the first ground object to the farthest position
            GameObject firstGround = groundPool[0];
            float farthestZ = FindFarthestZ();
            firstGround.transform.position = new Vector3(0f, 0f, farthestZ + spawnDistance + firstGround.transform.localScale.z);

            // Move the first ground object to the end of the pool
            groundPool.RemoveAt(0);
            groundPool.Add(firstGround);

            // Spawn the next ground object
            SpawnGround();
        }
    }

    private float FindFarthestZ()
    {
        float farthestZ = float.MinValue;
        foreach (GameObject ground in groundPool)
        {
            float groundZ = ground.transform.position.z;
            if (groundZ > farthestZ)
            {
                farthestZ = groundZ;
            }
        }
        return farthestZ;
    }

    private void SpawnGround()
    {
        // Get the next ground object from the pool
        GameObject groundToSpawn = groundPool[0];

        // Set the position for the ground object
        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
        groundToSpawn.transform.position = spawnPosition;

        // Activate the ground object
        groundToSpawn.SetActive(true);

        // Move the spawned ground object to the end of the pool
        groundPool.RemoveAt(0);
        groundPool.Add(groundToSpawn);

        // Calculate the position for the next ground spawn
        nextSpawnZ += spawnDistance;
    }

    private GameObject GetRandomGroundPrefab()
    {
        int randomIndex = Random.Range(0, groundPrefabs.Count);
        return groundPrefabs[randomIndex];
    }
}
