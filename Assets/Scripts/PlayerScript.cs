using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Player  values
    public Animator animator;
    public float playerMovementSpeed = 10f;
    private Vector2 playerMovementDirection;
    public int playerMaxHealth = 100;
    public int playerHealth;
    private bool playerFacingRight = true;

    // Logicv manager
    public GameLogicManagerScript gameLogicManager;

    // Rigid body object of the player character
    public Rigidbody2D rigidBody;

    // Weapon
    public PlayerWeaponScript playerWeaponScript;

    // Upgrade manager for the values
    public UpgradeManagerScript upgradeManagerScript;

    // Current projectile values
    public Projectile currentPlayerProjectile;

    // Upgrades assigned to player
    public List<Upgrade> playerUpgrades = new List<Upgrade>() { };

    // Audios
    public AudioSource deathSource;
    public AudioSource upgradeSource;

    // SpinningProjectile
    public SpinningProjectile spinningProjectile;


    // Start is called before the first frame update
    void Start()
    {
        // Get the scripts
        gameLogicManager = GameObject.FindGameObjectWithTag("GameLogicManager").GetComponent<GameLogicManagerScript>();
        playerWeaponScript = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<PlayerWeaponScript>();
        upgradeManagerScript = FindObjectOfType<UpgradeManagerScript>();
        spinningProjectile = GameObject.FindGameObjectWithTag("SpinningProjectile").GetComponent<SpinningProjectile>();

        // Assign saved values from switching the scenes
        if (GameData.playerHealth == 0)
        {
            GameData.playerHealth = GameData.playerMaxHealth;
        }
        playerHealth = GameData.playerHealth;
        playerMaxHealth = GameData.playerMaxHealth;
        currentPlayerProjectile = GameData.currentPlayerProjectile;
        spinningProjectile.SetShouldSpin(GameData.spinnerIsActive);
        // Update the text
        gameLogicManager.UpdatePlayerHealthText(GameData.playerHealth, GameData.playerMaxHealth);
        gameLogicManager.ShownPlayerLevel();
        gameLogicManager.ShownPlayerScore();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the player keyboard inputs and updates time
        ProcessPlayerInputs();
        gameLogicManager.UpdateTime();
    }

    // Calls after every input
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Player keyboard inputs
    void ProcessPlayerInputs()
    {
        if (playerHealth <= 0)
        {
            return;
        }
        // Get player movements
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var horizontalMovement = Input.GetAxisRaw("Horizontal");

        // Assing moving direction of the player
        // Normalized is used to keep z-axis velocity same as xy-axis velocity
        playerMovementDirection = new Vector2(horizontalMovement, verticalMovement).normalized;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement));

        //flip sprite: https://www.youtube.com/watch?v=Cr-j7EoM8bg
        if (horizontalMovement > 0f && !playerFacingRight)
        {
            FlipObject();
        }
        if (horizontalMovement < 0f && playerFacingRight)
        {
            FlipObject();
        }

        // Fire the weapon if mouse left click is pressed
        if (Input.GetMouseButtonDown(0))
        {
            playerWeaponScript.PlayerWeaponFire(GameData.currentPlayerProjectile.Type);
        }
    }

    void FlipObject()
    {
        // Geth the current scale (direction) and swith it around
        var currentScale = gameObject.transform.localScale;
        currentScale.x = currentScale.x * -1;
        gameObject.transform.localScale = currentScale;
        playerFacingRight = !playerFacingRight;
    }

    // Player movement
    void MovePlayer()
    {
        if (playerHealth <= 0)
        {
            return;
        }
        // Get the new velocity values
        var verticalVelocity = playerMovementDirection.x * GameData.playerMovementSpeed;
        var horizontalVelocity = playerMovementDirection.y * GameData.playerMovementSpeed;

        // Assing new velocity values
        rigidBody.velocity = new Vector2(verticalVelocity, horizontalVelocity);

    }

    // Destory the player
    void DestroyPlayer()
    {
        Destroy(gameObject);
        GameData.fullRun = false;
        gameLogicManager.GameOver();
    }

    // Take damage
    public void TakeDamage(int damageAmount)
    {
        playerHealth = GameData.playerHealth;
        playerHealth -= damageAmount;
        gameLogicManager.UpdatePlayerHealthText(playerHealth, playerMaxHealth);
        if (playerHealth <= 0)
        {
            animator.SetBool("Alive", false);
            AudioSource.PlayClipAtPoint(deathSource.clip, transform.position);
            Invoke("DestroyPlayer", 1); // destroy player after the death animation
        }
    }

    // Assing regulaer upgrade after level up
    public void GetUpgrade()
    {
        if (playerHealth <= 0)
        {
            return;
        }
        playerMaxHealth = GameData.playerMaxHealth;
        playerHealth = GameData.playerHealth;

        // Get normal upgrades
        var upgradeList = upgradeManagerScript.normalUpgrades;

        // Get random upgrade from 3 selections
        System.Random random = new System.Random();
        int index = random.Next(0, upgradeList.Count);
        var randomItem = upgradeList[index];

        // Assing the upgrade and play sound
        AssingUpgrade(randomItem);
        upgradeSource.Play();


    }

    // Assign the upgrade
    public void AssingUpgrade(NormalUpgrade upgrade)
    {
        if (playerHealth <= 0)
        {
            return;
        }
        switch (upgrade.Type)
        {
            case NormalUpgradesTypes.Speed:
                AddPlayerSpeed(upgrade.Value);
                break;
            case NormalUpgradesTypes.Health:
                AddPlayerHealth((int)upgrade.Value);
                break;

            case NormalUpgradesTypes.Damage:
                upgradeManagerScript.AddDamage((int)upgrade.Value);
                // Add damage to the current projectile
                GameData.currentPlayerProjectile.Damage += (int)upgrade.Value;
                break;
            default:
                break;
        }
        // Show the new upgrade to the player
        gameLogicManager.AddPlayerActionLog($"New upgrade: {upgrade.Name}\t {upgrade.Value}");

    }

    // Add player health
    public void AddPlayerHealth(int healthValue)
    {
        if (playerHealth <= 0)
        {
            return;
        }
        playerHealth = GameData.playerHealth;
        playerMaxHealth = GameData.playerMaxHealth;
        playerHealth += healthValue;

        // health cannot be over max health
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        };

        // Update the text
        gameLogicManager.UpdatePlayerHealthText(playerHealth, playerMaxHealth);
    }

    // Upgrades after killing the boss
    public void HandleUpgrade(Upgrade newUpgrade)
    {
        if (playerHealth <= 0)
        {
            return;
        }
        switch (newUpgrade.Type)
        {
            // Steals health
            case UpgradesTypes.DrainSoul:
                playerUpgrades.Add(newUpgrade);
                GameData.playerUpgrades.Add(newUpgrade);
                break;

            // Larger projectile
            case UpgradesTypes.AreaShot:
                currentPlayerProjectile = new Projectile
                {
                    Damage = (int)newUpgrade.Value,
                    Type = ProjectileType.AreaShot
                };
                upgradeManagerScript.ProjectileValues = currentPlayerProjectile;
                GameData.currentPlayerProjectile = currentPlayerProjectile;
                break;

            // Better default projectile
            case UpgradesTypes.UpgradedProjectile:
                currentPlayerProjectile = new Projectile
                {
                    Damage = GameData.currentPlayerProjectile.Damage,
                    Type = ProjectileType.Upgraded
                };
                upgradeManagerScript.ProjectileValues = currentPlayerProjectile;
                GameData.currentPlayerProjectile = currentPlayerProjectile;
                break;

            // Get new spinning projectile
            case UpgradesTypes.SpinningProjectile:
                spinningProjectile.SetShouldSpin(true);
                break;

            // Fully heal
            case UpgradesTypes.FullHealth:
                AddPlayerHealth(playerMaxHealth);
                break;

            // Upgrade all abilities
            case UpgradesTypes.AbilityIncrease:
                AddAllAbilityIncrease((int)(playerHealth * newUpgrade.Value), (int)(playerHealth * newUpgrade.Value), (int)(GameData.playerMovementSpeed * 1.1 - GameData.playerMovementSpeed), (int)(GameData.currentPlayerProjectile.Damage * 1.5));
                break;
            default:
                break;
        }

        // Play audio and log the upgrade
        upgradeSource.Play();
        gameLogicManager.AddPlayerActionLog($"New upgrade: {newUpgrade.Name}");

    }

    // Add speed
    public void AddPlayerSpeed(float speedValues)
    {
        if (playerHealth <= 0)
        {
            return;
        }
        GameData.playerMovementSpeed += speedValues;
    }

    // Add to max health
    public void AddMaxHealth(int maxHealth)
    {
        if (playerHealth <= 0)
        {
            return;
        }
        GameData.playerMaxHealth += maxHealth;
    }

    // Add to all abilities
    public void AddAllAbilityIncrease(int health, int maxHealth, float speed, int damage)
    {
        if (playerHealth <= 0)
        {
            return;
        }
        AddMaxHealth(maxHealth);
        AddPlayerHealth(health);
        AddPlayerSpeed(speed);
        upgradeManagerScript.AddDamage(damage); // this might not be needed
        GameData.currentPlayerProjectile.Damage = damage;

    }
}
