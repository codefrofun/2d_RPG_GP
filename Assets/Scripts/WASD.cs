using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WASD : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile playerTile;
    public TileMap tileMapLoaderScript;
    public Tile enemyTile;

    private Vector3Int playerTilePosition;
    private Vector3Int enemyTilePosition;

    private EnemyScript enemyScript;

    private bool isMoving = false;
    private bool isInRoomTransition = false;
    private bool enemyTurn = false;

    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        if (tileMapLoaderScript == null)
        {
            Debug.LogError("TileMapLoaderScript is not assigned in the Inspector!");
            return;  
        }

        playerTilePosition = tileMapLoaderScript.GetPlayerTilePosition();
    }

    void Update()
    {
        if (!isMoving && !isInRoomTransition && !enemyTurn)
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

        if (targetTile == tileMapLoaderScript.enemyTile)
        {
            AttackEnemy(targetTilePosition);
            return;
        }

        if (targetTile != null && targetTile != tileMapLoaderScript.wallTile && targetTile != tileMapLoaderScript.chestTile && targetTile != tileMapLoaderScript.doorTile && targetTile != enemyTile)
        {
            isMoving = true;
            tilemap.SetTile(playerTilePosition, tileMapLoaderScript.floorTile);
            tilemap.SetTile(targetTilePosition, playerTile);
            playerTilePosition = targetTilePosition;
            tileMapLoaderScript.SetPlayerTilePosition(playerTilePosition);

            enemyTurn = true;

            Invoke("MoveEnemyTowardPlayer", 0.5f);

            StartCoroutine(MovementDelay());
        }
        else if (targetTile == tileMapLoaderScript.doorTile && !isInRoomTransition)
        {
            isInRoomTransition = true;
            tileMapLoaderScript.LoadMap();
            playerTilePosition = tileMapLoaderScript.GetPlayerTilePosition();
            tilemap.SetTile(playerTilePosition, playerTile);
            enemyScript.health = 20;
            MoveEnemyTowardPlayer();
            StartCoroutine(MovementDelay());
        }
        else if(targetTile == enemyTile)
        {
            Debug.Log("Enemy has beem attacked");
        }
        else
        {
            Debug.Log("You can't walk there!");
        }
    }

    void AttackEnemy(Vector3Int enemyPosition)
    {
        Debug.Log("Player attacked the enemy");

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(20);

            if (enemyScript.health <= 0)
            {
                tilemap.SetTile(enemyPosition, tileMapLoaderScript.floorTile);
                tileMapLoaderScript.SetEnemyTilePosition(Vector3Int.zero);
                enemyTilePosition = Vector3Int.zero;
                enemyScript = null;

                enemyTurn = false;
                isMoving = false;

                playerTile.SetCanMove(true);
                playerTile.heal(10);
                GameScoreUpdate.AddLevel(1);
            }
        }
            
    }

    void MoveEnemyTowardPlayer()
    {
        if (enemyScript == null || enemyScript.health <= 0)
        {
            enemyTurn = false;
            return;
        }

        Vector3Int enemyTilePosition = tileMapLoaderScript.GetEnemyTilePosition();

        Vector3Int direction = Vector3Int.zero;

        if (playerTilePosition.x > enemyTilePosition.x)
        {
            direction.x = 1;
        }
        else if (playerTilePosition.x < enemyTilePosition.x)
        {
            direction.x = -1;
        }
        else if (playerTilePosition.y > enemyTilePosition.y)
        {
            direction.y = 1;
        }
        else if (playerTilePosition.y < enemyTilePosition.y)
        {
            direction.y = -1;
        }

        Vector3Int newEnemyPosition = enemyTilePosition + direction;

        TileBase tileAtNewPosition = tilemap.GetTile(newEnemyPosition);
        if (tileAtNewPosition != tileMapLoaderScript.wallTile && tileAtNewPosition != tileMapLoaderScript.doorTile && tileAtNewPosition != tileMapLoaderScript.chestTile && tileAtNewPosition != playerTile)
        {
            tilemap.SetTile(enemyTilePosition, tileMapLoaderScript.floorTile);
            tileMapLoaderScript.SetEnemyTilePosition(newEnemyPosition);
            tilemap.SetTile(newEnemyPosition, tileMapLoaderScript.enemyTile);
        }
        enemyTurn = false;
    }

    IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isMoving = false;

        if (isInRoomTransition)
        {
            isInRoomTransition = false;
        }
    }

    public Vector3Int GetEnemyTilePosition()
    {
        return enemyTilePosition;
    }

    public void SetEnemyTilePosition(Vector3Int newPosition)
    {
        enemyTilePosition = newPosition;
    }
}