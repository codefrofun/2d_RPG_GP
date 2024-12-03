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

    public HealthSystem()
    {
        health = 100;
        lives = 3;
        level = 1;
    }

    public string ShowHUD()
    {
        return $"HP: {health}  Lives: {lives}  Level: {level}  \nStatus: {healthStatus}";
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            return;
        }
            health -= damage;
    }

    public void ResetGame()
    {
        Console.WriteLine("Startover");
        health = 100;
        lives = 3;
        UpdateHealthStatus();
    }

    private void UpdateHealthStatus()
    {
        if (health <= 10)
        {
            healthStatus = "You are badly hurt!";
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
