using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyScript : MonoBehaviour
{
    public int health = 100;
    public int damage = 10;
    public float attackCooldown = 1f;
    public float lastAttackTime = 0f;

    public TileMap tilemap;
    public Tile wallTile;
    private Vector3Int enemyTilePosition;
    private Vector3Int playerTilePosition;

    private void Start()
    {
        playerTilePosition = tilemap.WorldToCell(GameObject.FindGameObjectWithTag("Player").transform.position); 
        enemyTilePosition = tilemap.WorldToCell(transform.position);
    }

    void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            MoveTowardPlayer(); 
            lastAttackTime = Time.time;
        }
    }

    public void MoveTowardPlayer()
    {
        Vector3Int direction = playerTilePosition - enemyTilePosition; 
        direction = new Vector3Int(Mathf.Sign(direction.x), Mathf.Sign(direction.y), 0);
        Vector3Int targetTile = enemyTilePosition + direction;

        TileBase tileAtTargetPosition = tilemap.GetTile(targetTile); 

        if (tileAtTargetPosition != null && tileAtTargetPosition != wallTile)
        {
            tilemap.SetTile(enemyTilePosition, null);
            tilemap.SetTile(targetTile, tilemap.GetTile("Enemy"));
            enemyTilePosition = targetTile; // Update enemy's tile position }
                            
            if (enemyTilePosition == playerTilePosition)
            {
                AttackPlayer();
            }
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
        Debug.Log("Enemy is dead");

        Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(playerTilePosition != null)
        {
            HealthSystem healthsystem = player.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                playerTilePosition.TakeDamage(damage);
            }
        }
    }
}
