using UnityEngine;

public class MinionScript : MonoBehaviour
{
    // Enemy values (overide from the unity GUI interface)
    public float movementSpeed = 5f;
    public int maxHealth = 50;
    public int health;
    public int damage = 10;
    public float lineOfSite = 10f;
    public int demonHeartCount = 1;

    // Player transform
    public Transform player;

    // Prefab of the deatheffect to spawn when the enemy dies
    public ParticleSystem deathEffectPrefab;

    // Prefab of the demon to spawn at the enemy's position
    public GameObject demonHeartObjectPrefab;

    // Death audio
    public AudioSource audioSource;

    // Start is called before the first frame update
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

            MoveTowardsPlayer();
        }
    }

    // Take damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            DestroyMinion();
        }
    }
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

    // Move towards the player: https://www.youtube.com/watch?v=2SXa10ILJms&list=WL&index=40
    private void MoveTowardsPlayer()
    {
        var playerPosition = player.transform.position;
        var minionPosition = transform.position;
        var distanceToPlayer = Vector2.Distance(playerPosition, minionPosition);
        // Player needs to be inrange
        if (distanceToPlayer < lineOfSite)
        {
            transform.position = Vector2.MoveTowards(minionPosition, playerPosition, movementSpeed * Time.deltaTime);
        }

    }

    // Get the player
    private void GetPlayer()
    {
        // Get the player if it exists
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    // Area of the lines of site (i.e when to start moving)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }

    // Collides with other object
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            DestroyMinion();
            player = null;
        }
    }
}
