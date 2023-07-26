using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    // Enemy values
    public Transform player;
    public int maxHealth = 50;
    public int health;
    public int damage = 10;
    public float rateOfFire = 0.5f;
    private float nextFireTime;
    public int demonHeartCount = 2;

    // Were the projectiles are fired
    public Transform firePoint;
    public GameObject enemyProjectile; // enemy projectile
    public ParticleSystem deathEffectPrefab; // Prefab of the effect to spawn when the enemy dies
    public GameObject demonHeartObjectPrefab; // Prefab of the object to spawn at the enemy's position


    // Audio
    public AudioSource audioSource;

    void Start()
    {
        // Assing the health, get the player and audio
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
        GetPlayer();

    }

    // Get the player: https://www.youtube.com/watch?v=N1BKXCxM_hA
    void Update()
    {
        if (!player)
        {
            GetPlayer();
        }
        else
        {

            ShootTowardsEnemy();
        }
    }

    // Take damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            DestroyMinion();
            player = null;
        }
    }

    // Destory the minion
    public void DestroyMinion()
    {
        // Play death sound
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        // Spawn replacement object at enemy's position
        for (int i = 0; i < demonHeartCount; i++)
        {
            Instantiate(demonHeartObjectPrefab, transform.position, Quaternion.identity);
        }

        // Show the death effect and destroy the game object
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    // Shoot towards the player:https://www.youtube.com/watch?v=lHLZxd0O6XY
    private void ShootTowardsEnemy()
    {
        if (nextFireTime < Time.time)
        {
            Instantiate(enemyProjectile, firePoint.position, Quaternion.identity);
            nextFireTime = Time.time + rateOfFire;
        }

    }

    private void GetPlayer()
    {
        // Get the player if it exists
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    // Collides with other object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
        }
    }
}
