using System.Linq;
using UnityEngine;

public class SpinningProjectile : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 10f;
    public float radius = 50f;
    public float rotationOffset = 90f;
    public bool spinnerIsActive;

    public GameObject spinningProjectilePrefab;
    public Transform spinningProjectileSpawnPoint;
    public int damage = 30;

    void Start()
    {
        gameObject.SetActive(true);
        spinnerIsActive = true;
        if (player is null)
        {
            player = FindObjectOfType<PlayerScript>().transform;
        }

        // Make the spinning object a child of the player
        transform.SetParent(player);
        transform.localPosition = Vector3.zero;

    }

    private void Update()
    {

        if (player == null || !GameData.spinnerIsActive)
        {
            return;
        }
        Spin();


    }

    public void SetShouldSpin(bool value)
    {
        spinnerIsActive = value;
        gameObject.SetActive(value);
        GameData.spinnerIsActive = value;
        if (value)
        {
            Spin();
        }

    }

    public void Spin()
    {
        // Calculate the desired angle
        float angle = (Time.time + rotationOffset) * rotationSpeed;

        // Calculate the new position of the spinning object relative to the player
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;

        // Update the position of the spinning object
        transform.position = player.position + offset;
    }

    // in hindsite probably just should have added the same tag to all minions and made the minion scirpt the similar to boss script
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
            || collision.gameObject.CompareTag("EnemyProjectile"))
        {
            return;
        }

        var playerScipt = FindObjectOfType<PlayerScript>();
        var drainSoul = GameData.playerUpgrades.FirstOrDefault(x => x.Type == UpgradesTypes.DrainSoul);
        if (drainSoul is not null)
        {
            playerScipt.AddPlayerHealth((int)drainSoul.Value);
        }

    }
}

