using UnityEngine;

public class BossSpawnerScript : MonoBehaviour
{
    public int bossSpawnLevel = 10; // Level at which the boss should spawn
    public GameObject bossPrefab; // Prefab of the boss object
    public bool spawned = false;

    public void CheckBossSpawn(int playerLevel)
    {
        if (playerLevel >= bossSpawnLevel && !spawned)
        {
            SpawnBoss();
            spawned = true;
        }
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }


}
