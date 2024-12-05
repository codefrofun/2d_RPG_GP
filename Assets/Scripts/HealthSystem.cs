using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public string healthStatus;
    public int lives;
    public int level;
    public bool isDead = false;
    public bool canMove = true;

    public int maxHealth;
    public int enemiesKilled = 0;

    public Slider healthSlider;
    public TMPro.TextMeshProUGUI killCountText;

    public HealthSystem()
    {
        health = 100;
        lives = 3;
        level = 0;
    }

    public void Start()
    {
        if (healthSlider == null)
        {
            Debug.LogError("Health slider is not assigned!");
        }
        else
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }

        if (healthSlider == null)
        {
            Debug.LogError("Health slider is not assigned!");
        }
        else
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
        EnemyKilled();
        UpdateKillCountUI();

        Debug.Log("Player Health: " + health);
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
        enemiesKilled = 0;
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

    void updateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled == 1)
        {
            DropKey();
        }
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = "Kills: " + enemiesKilled;
        }
    }

    private void DropKey()
    {
        Debug.Log("Key dropped!");
    }

    public void Die()
    {
        isDead = true;
        canMove = false;
        Debug.Log("Player has died");

        Debug.Log("Player is dead, deactivating object: " + gameObject.activeSelf);

        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.0f);
        // game over panel display
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