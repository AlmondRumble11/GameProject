using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    public GameLogicManagerScript gameLogicManager;
    public string sceneName;
    public bool getNewUpgrade = true;
    // Start is called before the first frame update
    void Start()
    {
        gameLogicManager = GameObject.FindGameObjectWithTag("GameLogicManager").GetComponent<GameLogicManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && gameLogicManager.levelBossIsDead)
        {

            if (SceneManager.GetActiveScene().name == "Arena3")
            {
                GameData.fullRun = true;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
