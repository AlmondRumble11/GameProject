using UnityEngine;

public class MovingAndShootingEnemy : MonoBehaviour
{
    public Transform player;
    public int maxHealth = 50;
    public int health;
    public int damage = 10;
    public float rateOfFire;
    private float nextFireTime;
    public int demonHeartCount = 5;
    public float movementSpeed = 5f;
    private readonly float lineOfSite = 5f;
    // Were the projectiles are fired
    public Transform firePoint;
    public GameObject enemyProjectile; // enemy projectile
    public ParticleSystem deathEffectPrefab; // Prefab of the effect to spawn when the enemy dies
    public GameObject demonHeartObjectPrefab; // Prefab of the object to spawn at the enemy's position

    // Audio
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
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
            MoveTowardsPlayer();
            ShootTowardsEnemy();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            DestroyMinion();
            player = null;
        }
    }

    public void DestroyMinion()
    {
        // Spawn replacement object at enemy's position
        for (int i = 0; i < demonHeartCount; i++)
        {
            Instantiate(demonHeartObjectPrefab, transform.position, Quaternion.identity);
        }

        if (audioSource != null)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }
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

    // Move towards the player: https://www.youtube.com/watch?v=2SXa10ILJms&list=WL&index=40
    private void MoveTowardsPlayer()
    {
        var playerPosition = player.transform.position;
        var minionPosition = transform.position;
        var distanceToPlayer = Vector2.Distance(playerPosition, minionPosition);
        if (distanceToPlayer < lineOfSite)
        {
            transform.position = Vector2.MoveTowards(minionPosition, playerPosition, movementSpeed * Time.deltaTime);
        }

    }


    private void OnDrawGizmosSelected()
    {
        if (health > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lineOfSite);
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
