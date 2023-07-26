using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    // Buttons of the menu
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;

    public void Start()
    {
        // Assign listeners
        if (option1Button)
        {
            option1Button.onClick.AddListener(OnOption1Click);
        }
        if (option2Button)
        {
            option2Button.onClick.AddListener(OnOption2Click);
        }
        if (option3Button)
        {
            option3Button.onClick.AddListener(OnOption3Click);
        }
        if (option4Button)
        {
            option4Button.onClick.AddListener(OnOption4Click);
        }



    }

    // Start the game
    public void OnOption1Click()
    {
        // Reset values to default
        GameData.playerLevel = GameDataDefault.playerLevel;
        GameData.currentPlayerProjectile = GameDataDefault.currentPlayerProjectile;
        GameData.time = Time.time;
        GameData.fullRun = GameDataDefault.fullRun;
        GameData.score = GameDataDefault.score;
        GameData.playerUpgrades = GameDataDefault.playerUpgrades;
        GameData.playerMovementSpeed = GameDataDefault.playerMovementSpeed;
        GameData.playerMaxHealth = GameDataDefault.playerMaxHealth;
        GameData.playerHealth = GameDataDefault.playerHealth;
        GameData.spinnerIsActive = GameDataDefault.spinnerIsActive;

        // Switch scene
        SceneManager.LoadScene("Tutorial");
    }

    // Credits
    public void OnOption2Click()
    {
        SceneManager.LoadScene("Credits");
    }

    // Quit
    public void OnOption3Click()
    {
        // https://gamedevbeginner.com/how-to-quit-the-game-in-unity/
        // Functions differrently in different environments
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // Go home
    public void OnOption4Click()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
