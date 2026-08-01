using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyPrefab;
    [SerializeField] private GameObject[] waypoints;

    private void Start()
    {
        SpawnEnemy();
    }

    /// <summary>
    /// Spawns an enemy and passes it waypoints
    /// Then registers it with the GameManager so it can be disabled on the win
    /// </summary>
    private void SpawnEnemy()
    {
        EnemyBase spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        EnemyPatroller patroller = spawnedEnemy as EnemyPatroller;

        if (patroller != null)
        {
            patroller.SetWaypoints(waypoints);
        }

        GameManager.Instance.RegisterEnemy(spawnedEnemy);
    }
}