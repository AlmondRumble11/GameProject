using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// How to create a table: https://www.youtube.com/watch?v=iAbaqGYdnyI
// How to save data: https://www.youtube.com/watch?v=x0Wy7jQ7EFU
// Reference to how to add JSON to PlayerPrefs: ChatGTP, Search term: How to add JSON to PlayerPrefs.
// Also this conversation: https://www.reddit.com/r/Unity3D/comments/eakh1m/unity_tip_saving_data_with_json_and_playerprefs/
public class LeaderboardTable : MonoBehaviour
{
    // Template and the container for the leaderboard
    public Transform template;
    public Transform container;

    // Input textfield and the button
    public Text playerNameInput;
    public Button playerInputButton;

    // Home button
    public Button goHomeButton;

    // Scores
    public List<ScoreData.ScoreDataContainer> scoreList = new List<ScoreData.ScoreDataContainer>() { };

    // Game over text
    public Text gameOverText;

    // Victory song
    public AudioSource victorySong;

    // Game over song
    public AudioSource gameOverSong;
    private void Awake()
    {
        if (gameOverText && GameData.fullRun)
        {
            gameOverText.text = "Congratulations you have beaten the game!";
            if (victorySong)
            {
                victorySong.Play();
            }

        }
        else
        {
            if (gameOverSong)
            {
                gameOverSong.Play();
            }

        }
        var storedScores = PlayerPrefs.GetString("leaderboardScoresKey");
        if (!string.IsNullOrEmpty(storedScores))
        {
            var jsonScores = JsonUtility.FromJson<ScoreDataWrapper>(storedScores);
            scoreList = jsonScores.allScores;
        }
        playerInputButton.onClick.AddListener(AddNewScore);
        goHomeButton.onClick.AddListener(GoHome);
        UpdateLeaderboard();
    }

    public void AddNewScore()
    {
        var playerName = playerNameInput.text;
        var time = GameData.time;
        var fullRun = GameData.fullRun;
        var level = GameData.playerLevel;

        // Store player data in the database
        var playerData = new ScoreData
        {
            PlayerName = playerName,
            Time = time,
            FullRun = fullRun,
            Level = level
        };

        scoreList.Add(playerData.GetContainer());
        SaveScores();
        UpdateLeaderboard();

    }

    public void UpdateLeaderboard()
    {
        //container = transform.Find("tableContainer");
        //template = container.Find("tableTemplate");
        template.gameObject.SetActive(false);
        var templateheigh = 30f;
        var index = 0;
        foreach (var score in scoreList.OrderBy(x => !x.fullRun).ThenByDescending(x => x.level).ThenBy(x => x.time).ToList())
        {
            // Add new height
            var transform = Instantiate(template, container);
            var rectTransform = transform.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -templateheigh * index);
            transform.gameObject.SetActive(true);
            index++;
            // Add list values
            transform.Find("posText").GetComponent<Text>().text = index.ToString();
            transform.Find("nameText").GetComponent<Text>().text = score.playerName?.ToString();
            transform.Find("timeText").GetComponent<Text>().text = FormatTime(score.time);
            transform.Find("completionText").GetComponent<Text>().text = score.fullRun.ToString();
            transform.Find("levelText").GetComponent<Text>().text = score.level.ToString();


            // Max lines is 15. Otherwise goes over the screen height limit
            if (index == 15)
            {
                break;
            }
        }
    }

    // Format the time. Same code as in the gamelogic manager but for some reason calling it in this script some times crashed the game
    public string FormatTime(float time)
    {
        // Convert the time to minutes and seconds
        var minutes = Mathf.FloorToInt(time / 60f);
        var seconds = Mathf.FloorToInt(time % 60f);
        var formattedTime = minutes.ToString("00") + ":" + seconds.ToString("00");

        return formattedTime;
    }

    // Go home button pressed
    public void GoHome()
    {
        SceneManager.LoadScene("StartMenu");
    }

    // Save scores if destroyed
    private void OnDestroy()
    {
        SaveScores();
    }

    // Save the scores to the playerPrefs
    public void SaveScores()
    {
        // Create a wrapper so JSON -> string conversion works.
        // Added empty object without this
        var wrapper = new ScoreDataWrapper
        {
            allScores = scoreList.Select(score => score).ToList()
        };
        var scoresAsJson = JsonUtility.ToJson(wrapper);

        // Save to player prefs. Not the best practice but could not install MongoDb nugets for unity for some reason so went with this
        PlayerPrefs.SetString("leaderboardScoresKey", scoresAsJson);
        PlayerPrefs.Save();
    }
}


// Score data to the leaderboards table
[System.Serializable]
public class ScoreData
{
    public string PlayerName { get; set; } = string.Empty;
    public float Time { get; set; }
    public bool FullRun { get; set; }
    public int Level { get; set; }

    // This is to get the score data for the JSON -> string conversion
    public ScoreDataContainer GetContainer()
    {
        return new ScoreDataContainer
        {
            playerName = PlayerName,
            time = Time,
            fullRun = FullRun,
            level = Level
        };
    }

    // Replicates the Scoredata values
    [System.Serializable]
    public class ScoreDataContainer
    {
        public string playerName;
        public float time;
        public bool fullRun;
        public int level;
    }
}
// For the wrapper
[System.Serializable]
public class ScoreDataWrapper
{
    public List<ScoreData.ScoreDataContainer> allScores;
}