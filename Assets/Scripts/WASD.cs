using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WASD : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile playerTile;
    private Vector3Int playerTilePosition;
    private bool isMoving = false;

    private TileMap tileMapScript;

    void Start()
    {
        tileMapScript = tilemap.GetComponent<TileMap>();
        playerTilePosition = tileMapScript.GetPlayerTilePosition();
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
        if (targetTile != null && targetTile != tileMapScript.wallTile && targetTile != tileMapScript.chestTile && targetTile != tileMapScript.doorTile)
        {
            isMoving = true;
            tilemap.SetTile(playerTilePosition, tileMapScript.floorTile);
            tilemap.SetTile(targetTilePosition, playerTile);
            playerTilePosition = targetTilePosition;
            StartCoroutine(MovementDelay());
        }
        else
        {
            Debug.Log("You can’t walk there!");
        }
    }

    IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isMoving = false;
    }
}
