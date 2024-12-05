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
    private WASD playerScript;
    private Vector3Int enemyTilePosition;
    private Vector3Int playerTilePosition;

    private bool isDead = false;



    private void Start()
    {
        if (playerScript == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Debug.Log("Looking for player: " + GameObject.FindGameObjectWithTag("Player"));

            if (playerScript != null)
            {
                playerScript = player.GetComponent<WASD>();

                if (player == null)
                {
                    Debug.LogError("WASD script not found on player! The enemy won't be able to interact with the player properly.");
                }
                else
                {
                    playerTilePosition = tilemap.WorldToCell(player.transform.position);
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
        // make code work here !! move enemy toward player
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Enemy Health: " + health);
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

    private void AttackPlayer()
    {
        if (playerScript == null)
        {
            playerScript.isMoving = true;
        }
    }
}