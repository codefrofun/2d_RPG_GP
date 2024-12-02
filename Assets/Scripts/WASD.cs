using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WASD : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile playerTile;
    public TileMap tileMapLoaderScript;
    public Vector3 spawn;

    private Vector3Int playerTilePosition;
    private bool isMoving = false;

    void Start()
    {
        if (tileMapLoaderScript == null)
        {
            Debug.LogError("TileMapLoaderScript is not assigned in the Inspector!");
            return;  // Stop the rest of the method if it's null
        }

        playerTilePosition = tileMapLoaderScript.GetPlayerTilePosition();
        //tileMapLoaderScript.map[1, tileMapLoaderScript.width / 2] = spawn;                !!!
    } 

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.W)) TryMove(Vector3Int.up);
            if (Input.GetKey(KeyCode.S)) TryMove(Vector3Int.down);
            if (Input.GetKey(KeyCode.A)) TryMove(Vector3Int.left);
            if (Input.GetKey(KeyCode.D)) TryMove(Vector3Int.right);
        }
    }

    void TryMove(Vector3Int direction)
    {
        Vector3Int targetTilePosition = playerTilePosition + direction;
        TileBase targetTile = tilemap.GetTile(targetTilePosition);

        if (targetTile != null && targetTile != tileMapLoaderScript.wallTile && targetTile != tileMapLoaderScript.chestTile && targetTile != tileMapLoaderScript.doorTile)
        {
            isMoving = true;
            tilemap.SetTile(playerTilePosition, tileMapLoaderScript.floorTile);
            tilemap.SetTile(targetTilePosition, playerTile);
            playerTilePosition = targetTilePosition;
            tileMapLoaderScript.SetPlayerTilePosition(playerTilePosition);
            StartCoroutine(MovementDelay());
        }
        else if(targetTile == tileMapLoaderScript.doorTile)
        {
            tileMapLoaderScript.LoadMap();
            // Reset player position               !!!
        }
        else
        {
            Debug.Log("You can't walk there!");
        }
    }

    IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isMoving = false;
    }
}
