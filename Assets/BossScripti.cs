using UnityEngine;

public class BossScripti : MonoBehaviour
{
    // Boss related values
    public int bossSpawnLevel = 10; // Level at which the boss should spawn
    public int maxHealth = 500;
    public int health;
    public int damage = 20;
    public string bossName;
    public bool phase1;
    public bool phase2;
    private bool phaseOneActivated = false;
    private bool phaseTwoActivated = false;

    // Player
    public Transform player;

    // Death particles
    public ParticleSystem deathEffectPrefab;

    // Movement and shooting related values
    public bool canMove;
    public bool canShoot;
    public float lineOfSite = 10f;
    public float movementSpeed = 10f;
    public float rateOfFire = 0.5f;
    private float nextFireTime;

    // Different firepoitns for projectiles
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;

    // Different projectile types
    public GameObject enemyProjectile; // enemy projectile
    public GameObject trackingProjectile; // enemy projectile

    // Audio
    public AudioSource audioSource;

    // Game logic manager
    public GameLogicManagerScript gameLogicManager;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        GetPlayer();
        gameLogicManager = GameObject.FindGameObjectWithTag("GameLogicManager").GetComponent<GameLogicManagerScript>();
    }

    // Get the player: https://www.youtube.com/watch?v=N1BKXCxM_hA
    void Update()
    {
        if (health <= 0)
        {
            return;
        }
        if (!player)
        {
            GetPlayer();
        }
        else
        {
            if (canMove)
            {
                MoveTowardsPlayer();
            }
            if (canShoot && (enemyProjectile || trackingProjectile))
            {
                ShootTowardsEnemy();
            }

        }
    }

    // Take damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            DestroyBoss();
        }

        // Phase 1 update for the boss
        if ((health <= maxHealth / 2) && phase1 && !phaseOneActivated)
        {
            damage += 10;
            phaseOneActivated = true;
            gameLogicManager.AddLogActionLog("Boss has enetered the 2nd phase. Damage has been increased");
        }

        // Phase 1 update for the boss
        if ((health <= maxHealth / 4) && phase2 && !phaseTwoActivated)
        {
            damage += 10;
            movementSpeed += 5;
            rateOfFire += 0.25f;
            phaseTwoActivated = true;
            gameLogicManager.AddLogActionLog("Boss has enetered the 3rd phase. Damage and speed and rate of fire have been increased");

        }
    }

    // Destroy the boss
    public void DestroyBoss()
    {
        // Play death audio
        if (audioSource != null)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }

        // Change level boss dead value to open scene swicher
        gameLogicManager.levelBossIsDead = true;
        gameLogicManager.AddLogActionLog($"{bossName} defeated! Go the next area");

        // Death expolsion and destroy the object
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        gameLogicManager.AssingSpecialUpgrade();
        Destroy(gameObject);

    }

    // Moving towards the player (reference link in minion script file)
    private void MoveTowardsPlayer()
    {
        if (health <= 0)
        {
            return;
        }
        var playerPosition = player.transform.position;
        var minionPosition = transform.position;
        var distanceToPlayer = Vector2.Distance(playerPosition, minionPosition);
        if (distanceToPlayer < lineOfSite)
        {
            transform.position = Vector2.MoveTowards(minionPosition, playerPosition, movementSpeed * Time.deltaTime);
        }

    }

    // Ge the player
    private void GetPlayer()
    {
        // Get the player if it exists
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    // Line of site area
    private void OnDrawGizmosSelected()
    {
        if (health > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lineOfSite);
        }

    }



    // Shoot towards the player:https://www.youtube.com/watch?v=lHLZxd0O6XY
    private void ShootTowardsEnemy()
    {
        if (health <= 0)
        {
            return;
        }
        if (nextFireTime < Time.time)
        {
            if (firePoint1 && enemyProjectile)
            {
                Instantiate(enemyProjectile, firePoint1.position, Quaternion.identity);
            }
            if (firePoint2 && enemyProjectile)
            {
                Instantiate(enemyProjectile, firePoint2.position, Quaternion.identity);
            }

            if (firePoint3 && trackingProjectile)
            {
                Instantiate(trackingProjectile, firePoint3.position, Quaternion.identity);
            }
            if (firePoint4 && trackingProjectile)
            {
                Instantiate(trackingProjectile, firePoint4.position, Quaternion.identity);
            }
            nextFireTime = Time.time + rateOfFire;
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
