using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array of tile prefabs to instantiate
    public Transform player; // Reference to the player's transform
    public float tileLength = 10f; // Length of each tile prefab
    public int numTilesOnScreen = 4; // Number of tiles to keep on screen

    private List<GameObject> activeTiles; // List of currently active tile prefabs
    private int lastTileIndex = 0; // Index of the last tile prefab spawned

    private Vector3 playerStartPosition; // Initial position of the player

    private void Start()
    {
        activeTiles = new List<GameObject>();

        // Store the initial position of the player
        playerStartPosition = player.position;

        // Instantiate the initial tiles incrementally
        for (int i = 0; i < numTilesOnScreen; i++)
        {
            SpawnTile(i);
        }
    }

    private void Update()
    {
        // Check if the player has passed the last two tiles
        if (player.position.z - numTilesOnScreen * tileLength > activeTiles[0].transform.position.z)
        {
            RepositionTile();
        }

        // Check for restart input
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restart");
            RestartGame();
        }
    }

    private void SpawnTile(int index)
    {
        GameObject tile = Instantiate(tilePrefabs[index]);
        tile.transform.SetParent(transform);
        tile.transform.position = Vector3.forward * (lastTileIndex * tileLength);
        lastTileIndex++;
        activeTiles.Add(tile);
    }

    private void RepositionTile()
    {
        GameObject tileToReposition = activeTiles[0];
        activeTiles.Remove(tileToReposition);

        // Randomly select the next tile prefab
        int randomIndex = Random.Range(0, tilePrefabs.Length);
        GameObject newTile = Instantiate(tilePrefabs[randomIndex]);
        newTile.transform.SetParent(transform);
        newTile.transform.position = Vector3.forward * (lastTileIndex * tileLength);
        lastTileIndex++;

        activeTiles.Add(newTile);

        // Destroy the repositioned tile
        Destroy(tileToReposition);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
