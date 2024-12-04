using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 100;
    public int damage = 10;
    public float attackCooldown = 1f;
    public float lastAttackTime = 0f;

    public TileMap tilemap;
    private Vector3Int enemyTilePosition;
    private Vector3Int playerTilePosition;

    private void Start()
    {
        playerTilePosition = tilemap.WorldToCell(GameObject.FindGameObjectWithTag("Player").transform.position); 
        enemyTilePosition = tilemap.WorldToCell(transform.position);
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
        if(playerTile != null)
        {
            Player HealthSystem = playerTile.GetComponent<HealthSystem>();
            if (HealthSystem != null)
            {
                playerTile.TakeDamage(damage);
            }
        }
    }
}
