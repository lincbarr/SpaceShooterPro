using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUps;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while(true)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            Vector3 posToSpawn = new Vector3(randomX, 7.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            float randomSeconds = Random.Range(3, 8);
            float randomX = Random.Range(-8.0f, 8.0f);
            Vector3 posToSpawn = new Vector3(randomX, 7.0f, 0);
            int randomPowerUp = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(randomSeconds);
        }
    }


    public void OnPlayerDeath()
    {
        Destroy(this.gameObject);  // Workaround for not being able to stop SpawnEnemyRoutine.
    }
}
