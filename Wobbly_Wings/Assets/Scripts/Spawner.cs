using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minSpawnRate = 1f; // Minimo intervallo tra gli spawn
    public float maxSpawnRate = 2f;  // Massimo intervallo tra gli spawn
    public float minHeight = -1f;    // Altezza minima dell'ostacolo
    public float maxHeight = 1f;     // Altezza massima dell'ostacolo

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
            Spawn();
            ScheduleNextSpawn();
        }
    }

    private void Spawn()
    {
        // Crea un nuovo ostacolo
        GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }

    private void ScheduleNextSpawn()
    {
        // Calcola un nuovo tempo di spawn randomico
        float spawnInterval = Random.Range(minSpawnRate, maxSpawnRate);
        nextSpawnTime = Time.time + spawnInterval;
    }
}
