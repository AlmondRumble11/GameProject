using System.Collections;
using UnityEngine;

// Enemy spawner : https://www.youtube.com/watch?v=SELTWo1XZ0c
public class EnemySpwanerScipt : MonoBehaviour
{
    // Prefabs
    [SerializeField]
    public GameObject minion1Prefab;
    [SerializeField]
    public GameObject minion2Prefab;
    [SerializeField]
    public GameObject major1Prefab;
    [SerializeField]
    public GameObject major2Prefab;

    // Interval between enemy spawns
    [SerializeField]
    public float minion1SpawnInterval = 200f;
    [SerializeField]
    public float minion2SpawnInterval = 500f;
    [SerializeField]
    public float major1SpawnInterval = 1000f;
    [SerializeField]
    public float major2SpawnInterval = 3000f;


    private void Start()
    {

        // Start the endless spawner
        if (minion1Prefab)
        {
            StartCoroutine(SpawnEnemy(minion1SpawnInterval, minion1Prefab));
        }
        if (minion2Prefab)
        {
            StartCoroutine(SpawnEnemy(minion2SpawnInterval, minion2Prefab));
        }
        if (major1Prefab)
        {
            StartCoroutine(SpawnEnemy(major1SpawnInterval, major1Prefab));
        }
        if (major2Prefab)
        {
            StartCoroutine(SpawnEnemy(major2SpawnInterval, major2Prefab));
        }

    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        // Add spawn new enemy to given position
        Instantiate(enemy, transform.position, Quaternion.identity);
        StartCoroutine(SpawnEnemy(interval, enemy));


    }
}
