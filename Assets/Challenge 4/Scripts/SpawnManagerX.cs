using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject bossPrefab; // NUEVO: Prefab del jefe

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 1;
    public float maxSpeed = 1.5f;

    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }

    }

    // Generate random spawn position for powerups and enemy balls
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }


    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15);

        // Spawn powerup si no hay
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // Spawn enemigos normales
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        // Si es la wave es multiplo de 3, también spawnea el jefe
        if (waveCount % 3 == 0)
        {
            Debug.Log("¡Wave: Jefe + enemigos!");
            Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        }

        // Ajuste de velocidad
        List<EnemyX> listaEnemyInScene = new List<EnemyX>(FindObjectsByType<EnemyX>(FindObjectsSortMode.None));
        foreach (var enemy in listaEnemyInScene)
        {
            if (waveCount <= maxSpeed)
                enemy.speed += 0.5f;
            else
                enemy.speed = maxSpeed;
        }

        waveCount++;
        ResetPlayerPosition();
    }

    // Move player back to position in front of own goal
    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }

}
