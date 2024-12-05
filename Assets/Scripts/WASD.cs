using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WASD : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile playerTile;
    public TileMap tileMapLoaderScript;
    public Tile enemyTile;
    public Tile openedChestTile;


    public bool isMoving = false;

    private Vector3Int playerTilePosition;
    private Vector3Int enemyTilePosition;

    private EnemyScript enemyScript;
    private HealthSystem healthSystem;

    private bool isInRoomTransition = false;
    public bool enemyTurn = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            healthSystem = player.GetComponent<HealthSystem>();
            if (healthSystem == null)
            {
                Debug.LogError("HealthSystem script is not attached to the player object!");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }

        enemyScript = GetComponent<EnemyScript>();
        if (player != null)
        {
            healthSystem = player.GetComponent<HealthSystem>();
        }

        if (tileMapLoaderScript == null)
        {
            Debug.LogError("TileMapLoaderScript is not assigned in the Inspector!");
            return;
        }

        playerTilePosition = tileMapLoaderScript.GetPlayerTilePosition();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is missing in the Inspector!");
        }

        if (tileMapLoaderScript == null)
        {
            Debug.LogError("TileMapLoaderScript reference is missing in the Inspector!");
        }
    }

    void Update()
    {
        if (healthSystem != null && !healthSystem.canMove)
        {
            return;
        }

        if (enemyScript != null && enemyScript.health <= 0)
        {
            enemyTurn = false;
            return;
        }

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

        if (targetTile == tileMapLoaderScript.chestTile)
        {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            if (playerInventory != null && playerInventory.hasKey)
            {
                tilemap.SetTile(targetTilePosition, openedChestTile);
                playerInventory.hasKey = false;

                ChestWin chestWin = FindObjectOfType<ChestWin>();
                if (chestWin != null)
                {
                    chestWin.OpenChest();
                }

                Debug.Log("Chest opened! You win!");

                isMoving = false;

                Invoke("ReturnToMainMenu", 3f);
                return;
            }
            else
            {
                Debug.Log("You need a key to open the chest.");
                return;
            }
        }

        if (targetTile != null && targetTile != tileMapLoaderScript.wallTile && targetTile != tileMapLoaderScript.doorTile && targetTile != enemyTile)
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
            enemyScript.health = 100;
            enemyTurn = false;
            isInRoomTransition = false;

            MoveEnemyTowardPlayer();
            StartCoroutine(MovementDelay());

            isMoving = false;
        }

        else
        {
            Debug.Log("You can't walk there!");
        }
    }

    void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
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

                enemyTurn = false;
                isMoving = true;

                //playerTile.SetCanMove(true);
                if (healthSystem != null)
                {
                    healthSystem.Heal(10);
                }
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
        /* else if (direction.x = tileMapLoaderScript.wallTile || direction.y = walltile)
        {

        } */

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