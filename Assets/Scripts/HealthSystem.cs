using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public string healthStatus;
    public int shield;
    public int lives;
    public int xp;
    public int level;
    public bool preventRevive = false;

    public HealthSystem()
    {
        health = 100;
        shield = 100;
        lives = 3;
        level = 1;
        xp = 0;
    }

    public string ShowHUD()
    {
        return $"HP: {health}  Shield: {shield}  Lives: {lives} \nStatus: {healthStatus} EXP: {xp}  Level: {level}";
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            return;
        }

        if (shield > 0)
        {
            shield -= damage;
            if (shield < 0)
            {
                health += shield;
                shield = 0;
            }
        }
        else
        {
            health -= damage;
        }

        if (health <= 0)
        {
            Revive();
        }
        UpdateHealthStatus();
    }


    public void Heal(int hp)
    {
        if (hp < 0)
        {
            return;
        }

        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        UpdateHealthStatus();
    }

    public void RegenerateShield(int hp)
    {
        if (hp < 0)
        {
            return;
        }

        if (shield < 100)
        {
            shield += hp;
        }
        if (shield > 100)
        {
            shield = 100;
        }
    }

    public void Revive()
    {
        if (preventRevive)
            return; // Skip revive during tests

        if (lives > 0)
        {
            health = 100;
            shield = 100;
            lives--;
            UpdateHealthStatus();
        }
        else if (health == 0 && lives <= 0)
        {
            ResetGame();
        }
    }


    public void ResetGame()
    {
        Console.WriteLine("Startover");
        health = 100;
        shield = 100;
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

    // Optional XP system methods
    public void IncreaseXP(int exp)
    {
        xp += exp;
        while (xp >= 100)
        {
            if (level < 99)
            {
                level++;
                xp -= 100;
            }
            else
            {
                xp = 100;
                break;
            }
        }
    }
}
