using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoreUpdate : MonoBehaviour
{
    public int lives;
    public int healthStatus;
    public int level;

    public void AddLevel(int amount)
    {
        level += amount;
    }
}
