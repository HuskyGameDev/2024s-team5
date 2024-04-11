using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] enemyPrefabs;
    private bool canSpawn = true;
    private int diffMod = 0;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;

            Spawn();
        }
    }

    private void Spawn()
    {
        int rand = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[rand];
        for (int i = 0; i < Random.Range(1 + (diffMod * 0.1f), 5 + (diffMod * 0.1f)); i++){
            Instantiate(enemyToSpawn, new Vector2(Random.Range(-29, 30), Random.Range(-29, 30)), Quaternion.identity);
        }
        diffMod += 1;
    }
}
