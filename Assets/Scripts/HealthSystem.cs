using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public string healthStatus;
    public int lives;
    public int level;
    public bool isDead = false;

    public HealthSystem()
    {
        health = 100;
        lives = 3;
        level = 0;
    }

    public string ShowHUD()
    {
        return $"HP: {health}  Lives: {lives}  Level: {level}  \nStatus: {healthStatus}";
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void ResetGame()
    {
        Console.WriteLine("Startover");
        health = 100;
        lives = 3;
        UpdateHealthStatus();
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > 100)
        {
            health = 100;
        }
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("Player has died");
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.0f);
        // game over
    }

    private void UpdateHealthStatus()
    {
        if (health <= 10)
        {
            healthStatus = "Near Death";
        }
        else if (health <= 50)
        {
            healthStatus = "Half health";
        }
        else if (health <= 75)
        {
            healthStatus = "You are hurt";
        }
        else if (health <= 90)
        {
            healthStatus = "Damage taken";
        }
        else
        {
            healthStatus = "Full Health";
        }
    }
}