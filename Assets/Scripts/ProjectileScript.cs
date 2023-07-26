using System.Linq;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage = 10;
    public PlayerScript player;
    public ProjectileType type;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {

        // player audio
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Start the destory timer
        Invoke("DestroyProjectile", lifeTime);

        // Get the player
        player = FindObjectOfType<PlayerScript>();
        if (player)
        {

            damage = GameData.currentPlayerProjectile.Damage;
        }

    }

    // Move the projectile
    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // destory the projectile
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    // Collides with other object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage an enemy
        if (collision.gameObject.CompareTag("Minion1") || collision.gameObject.CompareTag("Major1"))
        {
            collision.gameObject.GetComponent<MinionScript>().TakeDamage(damage);

        }
        if (collision.gameObject.CompareTag("Minion2") || collision.gameObject.CompareTag("Major2"))
        {
            collision.gameObject.GetComponent<ShootingEnemy>().TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag("Boss1") || collision.gameObject.CompareTag("Boss2") || collision.gameObject.CompareTag("Boss3"))
        {
            collision.gameObject.GetComponent<BossScripti>().TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag("PlayerWeapon")
            || collision.gameObject.CompareTag("Player")
            || collision.gameObject.CompareTag("Portal")
            || collision.gameObject.CompareTag("DemonHeart")
            || collision.gameObject.CompareTag("SpinningProjectile")
            || collision.gameObject.CompareTag("EnemyProjectile"))
        {
            return;
        }

        /// Drain soul
        var playerScipt = FindObjectOfType<PlayerScript>();
        var drainSoul = playerScipt.playerUpgrades.FirstOrDefault(x => x.Type == UpgradesTypes.DrainSoul);
        if (drainSoul is not null)
        {
            playerScipt.AddPlayerHealth((int)drainSoul.Value);
        }

        // Destory the projectile
        if (type == ProjectileType.Normal || collision.gameObject.CompareTag("Wall"))
        {
            DestroyProjectile();
        }

    }


}
