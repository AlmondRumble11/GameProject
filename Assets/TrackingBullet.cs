using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Transform player;
    public int damage = 10;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    // Move towards the player: https://www.youtube.com/watch?v=2SXa10ILJms&list=WL&index=40
    private void MoveTowardsPlayer()
    {
        if (player)
        {
            var playerPosition = player.transform.position;
            var projectilePosition = transform.position;
            transform.position = Vector2.MoveTowards(projectilePosition, playerPosition, movementSpeed * Time.deltaTime);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            DestroyProjectile();
        }

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Projectile"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
