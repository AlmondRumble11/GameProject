using UnityEngine;


// Projectile move towards player: https://www.youtube.com/watch?v=lHLZxd0O6XY
public class EnemyProjectile : MonoBehaviour
{

    public float speed;
    public float lifeTime = 5f;
    public GameObject projectileDestroyEffect;
    public int damage = 5;
    public GameObject player;
    public Rigidbody2D projectileRigidbody;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {

        // Shoot towards player
        player = GameObject.FindGameObjectWithTag("Player");
        var playerPosition = player.transform.position;
        var projectilePosition = transform.position;
        var direction = (playerPosition - projectilePosition).normalized * speed;
        projectileRigidbody.velocity = new Vector2(direction.x, direction.y);

        // Rotate towards player
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        // Play sound
        audioSource.Play();

        // Destroy timer
        Invoke("DestroyProjectile", lifeTime);


    }


    // Collides with other object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            DestroyProjectile();
        }

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("SpinningProjectile"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
