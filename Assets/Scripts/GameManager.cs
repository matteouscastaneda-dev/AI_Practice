using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform PlayerTransform { get; private set; }

    [SerializeField] private PlayerController player;

    private List<EnemyBase> enemies = new List<EnemyBase>();

    private bool hasWon;
    public bool HasKey;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        PlayerTransform = player.transform;
    }

    /// <summary>
    /// Adds each spawned enemy to the list so it can be disabled when the level is won
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(EnemyBase enemy)
    {
        if (enemy == null || enemies.Contains(enemy))
        {
            return;
        }

        enemies.Add(enemy);
    }

    /// <summary>
    /// Sends the player back to their starting position
    /// </summary>
    public void ResetPlayer()
    {
        if (player == null || hasWon)
        {
            return;
        }

        player.ResetPosition();
    }

    /// <summary>
    /// Checks for key then ends the game and freezes every enemy
    /// </summary>
    public void TryExit()
    {
        if (!HasKey || hasWon)
        {
            return;
        }

        hasWon = true;
        DisableAllEnemies();
        Debug.Log("Game Dont");
    }

    /// <summary>
    /// Switches off every enemy navmeshagent
    /// </summary>
    private void DisableAllEnemies()
    {
        for (int enemy = 0; enemy < enemies.Count; enemy++)
        {
            if (enemies[enemy] == null)
            {
                continue;
            }

            NavMeshAgent enemyAgent = enemies[enemy].GetComponent<NavMeshAgent>();
            enemies[enemy].enabled = false;
        }
    }
}