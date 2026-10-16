
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public int maxObstacles = 6;
    public float minPlayerDistance = 2f;

    [Header("Spawn Area")]
    public Vector2 minPosition = new Vector2(-7.5f, -3.5f);
    public Vector2 maxPosition = new Vector2(7.5f, 3.5f);

    void Start()
    {
        InvokeRepeating(
            nameof(SpawnObstacle),
            1f,
            spawnInterval
        );
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
            return;

        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        if (obstacles.Length >= maxObstacles)
            return;

        for (int i = 0; i < 20; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );

            if (player != null)
            {
                float distance = Vector2.Distance(
                    spawnPosition,
                    player.position
                );

                if (distance < minPlayerDistance)
                    continue;
            }

            if (Physics2D.OverlapCircle(spawnPosition, 0.7f))
                continue;

            Instantiate(
                obstaclePrefab,
                spawnPosition,
                Quaternion.identity
            );

            break;
        }
    }
}
