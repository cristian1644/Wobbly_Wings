using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab; // Prefab del power-up
    public float minSpawnRate = 10f; // Minimo intervallo tra gli spawn
    public float maxSpawnRate = 50f;  // Massimo intervallo tra gli spawn
    public float spawnY = 1f;
    public float spawnX = 5f; // Posizione orizzontale del power-up

    private float nextSpawnTime;

    private void OnEnable()
    {
        ScheduleNextSpawn();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        // Controlla se è il momento di spawnare un nuovo ostacolo
        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerUp();
            ScheduleNextSpawn();
        }
    }

    private void SpawnPowerUp()
    {
        // Genera una posizione casuale
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Controlla che non ci siano collisioni con le pipes
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0.5f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Obstacle")) // Assicurati che le pipes abbiano il tag "Pipe"
            {
                Debug.Log("spawn annullato");
                return; // Non spawna il power-up se c'è una pipe
            }
        }

        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }

    private void ScheduleNextSpawn()
    {
        // Calcola un nuovo tempo di spawn randomico
        float spawnInterval = Random.Range(minSpawnRate, maxSpawnRate);
        nextSpawnTime = Time.time + spawnInterval;
    }
}
