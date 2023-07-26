using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogicManagerScript : MonoBehaviour
{

    // Public values
    public int score;
    public int playerLevel;
    public Text scoreText;
    public Text playerHealthText;
    public Text playerLevelText;
    public Text timeText;
    public GameObject gameOverScreen;
    public bool playerIsAlive = true;
    public BossSpawnerScript bossSpawnerScript;
    public PlayerScript playerScript;
    public bool levelBossIsDead = false;
    public ActionLog actionLog;


    // Set the desired screen width and height
    public int targetScreenWidth = 1980;
    public int targetScreenHeight = 1080;

    // Set whether to run the game in fullscreen or not
    public bool fullscreen = false;

    private void Awake()
    {
        SetScreenResolution();
    }

    private void Start()
    {
        // Start the time
        UpdateTime();

        // Get scripts
        if (GameObject.FindGameObjectWithTag("BossSpawner"))
        {
            bossSpawnerScript = GameObject.FindGameObjectWithTag("BossSpawner").GetComponent<BossSpawnerScript>();
        }
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }
        if (GameObject.FindGameObjectWithTag("ActionLog"))
        {
            actionLog = GameObject.FindGameObjectWithTag("ActionLog").GetComponent<ActionLog>();
        }

    }

    private void SetScreenResolution()
    {
        // Set  screen resolution
        Screen.SetResolution(targetScreenWidth, targetScreenHeight, fullscreen);
    }

    // Updateds the time text
    public void UpdateTime()
    {
        // Get the current time
        var currentTime = Time.time;
        GameData.time = currentTime;
        // Convert the time to a formatted string
        var formattedTime = FormatTime(currentTime);
        if (timeText)
        {
            // Update text
            timeText.text = formattedTime;
        }

    }

    // Format the time to minutes:seconds format 
    public string FormatTime(float time)
    {
        // Convert the time to minutes and seconds
        var minutes = Mathf.FloorToInt(time / 60f);
        var seconds = Mathf.FloorToInt(time % 60f);
        var formattedTime = minutes.ToString("00") + ":" + seconds.ToString("00");

        return formattedTime;
    }

    // Add demon hearts (i.e score)
    public void AddDemonHeart(int scoreToAdd)
    {
        GameData.score += scoreToAdd;
        scoreText.text = GameData.score.ToString();
        // Player levels up after they have collected 10 hearts
        if (GameData.score % 10 == 0)
        {
            UpdatePlayerLevel(1);
        }
    }

    // Update the player health text
    public void UpdatePlayerHealthText(int newHealth, int maxHealth)
    {
        playerHealthText.text = $"{newHealth}/{maxHealth}";
        GameData.playerHealth = newHealth;
        GameData.playerMaxHealth = maxHealth;
    }

    // Update the player level text
    public void UpdatePlayerLevel(int level)
    {
        // Do not update the level if there is no boss in the level (tutorial, or the hallway scene)
        if (!bossSpawnerScript)
        {
            return;
        }
        GameData.playerLevel += level;
        playerLevelText.text = GameData.playerLevel.ToString();

        // Check if the player level is high enough for the boss to spawn
        bossSpawnerScript.CheckBossSpawn(GameData.playerLevel);

        // Assing new random upgrade after each level
        playerScript.GetUpgrade();
    }

    // Show the player level. Gets called when player switches scenes
    public void ShownPlayerLevel()
    {
        playerLevelText.text = GameData.playerLevel.ToString();
    }

    // Show the player score. Gets called when player switches scenes
    public void ShownPlayerScore()
    {
        scoreText.text = GameData.score.ToString();
    }

    // Go to game over screen
    public void GameOver()
    {
        playerIsAlive = false;
        SceneManager.LoadScene("Gameover");
    }

    // Get special upgrade after swiching scenes after boss is dead
    public void AssingSpecialUpgrade()
    {
        var upgradeManager = FindObjectOfType<UpgradeManagerScript>();
        var upgrades = upgradeManager.upgrades;
        System.Random random = new System.Random();
        int index = random.Next(0, upgradeManager.upgrades.Count);
        var randomItem = upgrades[index];
        playerScript.HandleUpgrade(randomItem);
    }

    // Add to quest log
    public void AddLogActionLog(string text)
    {
        actionLog.AddQuestLogMessage(text);
    }

    // Add to player log
    public void AddPlayerActionLog(string text)
    {
        actionLog.AddLogMessage(text);
    }

    // Remove score when player sends demon hearts to the upper world to reveice upgrades
    public void RemoveScore(int value)
    {
        GameData.score -= value;
        ShownPlayerScore();
    }
}
