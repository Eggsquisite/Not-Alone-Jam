using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPosLeft;
    [SerializeField] private Transform spawnPosRight;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    private float spawnTimer = 0f;
    private float chosenSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        chosenSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer();
    }

    private void SpawnTimer() {
        if (spawnTimer < chosenSpawnTime) {
            spawnTimer += Time.deltaTime;
        } else if (spawnTimer > chosenSpawnTime) {
            spawnTimer = chosenSpawnTime;
        } else if (spawnTimer == chosenSpawnTime) {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        var tmp = (int)Random.Range(1, 3);
        if (tmp == 1)
            Instantiate(enemyPrefab, new Vector2(spawnPosLeft.position.x, spawnPosLeft.position.y), Quaternion.identity);
        else if (tmp == 2)
            Instantiate(enemyPrefab, new Vector2(spawnPosRight.position.x, spawnPosRight.position.y), Quaternion.identity);

        spawnTimer = 0f;
        chosenSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
