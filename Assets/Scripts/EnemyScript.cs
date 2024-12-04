using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyScript : MonoBehaviour
{
    public int health = 100;
    public int damage = 10;
    public float attackCooldown = 1f;
    public float lastAttackTime = 0f;

    public Tilemap tilemap;
    public Tile wallTile;
    public Tile floorTile;
    private WASD playerScript;
    private Vector3Int enemyTilePosition;
    private Vector3Int playerTilePosition;

    private bool isDead = false;

    public float attackRange = 1f;

    private void Start()
    {
        if (playerScript == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                playerScript = player.GetComponent<WASD>();
                if (playerScript != null)
                {
                    playerTilePosition = tilemap.WorldToCell(player.transform.position);
                }
                else
                {
                    Debug.LogError("WASD script not found on player! The enemy won't be able to interact with the player properly.");
                }
            }
            else
            {
                Debug.LogError("Player not found, the enemy can't interact with player");
            }
        }

        enemyTilePosition = tilemap.WorldToCell(transform.position);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            MoveTowardPlayer();
            lastAttackTime = Time.time;
        }
    }

    public void MoveTowardPlayer()
    {
        Vector3 direction = playerTilePosition - enemyTilePosition;

        if (Mathf.Abs((int)direction.x) <= (int)attackRange && Mathf.Abs((int)direction.y) <= (int)attackRange)
        {
            AttackPlayer();
        }
        else
        {
            Vector3 moveDirection = Vector3.zero;

            if (Mathf.Abs((int)direction.x) > Mathf.Abs((int)direction.y))
            {
                moveDirection.x = Mathf.Sign(direction.x);
            }
            else
            {
                moveDirection.y = Mathf.Sign(direction.y);
            }

            Vector3Int newEnemyPosition = new Vector3Int((int)(enemyTilePosition.x + moveDirection.x),
                                                         (int)(enemyTilePosition.y + moveDirection.y),
                                                         (int)enemyTilePosition.z);

            TileBase tileAtNewPosition = tilemap.GetTile(newEnemyPosition);

            if (tileAtNewPosition != wallTile && tileAtNewPosition != playerScript.playerTile && tileAtNewPosition != null)
            {
                tilemap.SetTile(enemyTilePosition, floorTile);
                enemyTilePosition = newEnemyPosition;
                tilemap.SetTile(enemyTilePosition, wallTile);
            }
        }
    }


    private void AttackPlayer()
    {
        if (playerScript != null && playerScript.healthSystem != null)
        {
            playerScript.healthSystem.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        Debug.Log("Enemy is dead");

        if (playerScript != null)
        {
            playerScript.isMoving = true;
        }

        this.enabled = false;
        Destroy(gameObject);
    }
}