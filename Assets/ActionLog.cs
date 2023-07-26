using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionLog : MonoBehaviour
{
    // Text fields for the logs
    public Text logText;
    public Text questText;

    // Queue for the logs
    private readonly Queue<string> logMessages = new Queue<string>();
    private readonly Queue<string> questLogMessages = new Queue<string>();

    // Max log lines
    public int maxLogLines = 5;


    private void Awake()
    {
        // Keep the PlayerActionLog between scene changes
        // Some how got messed up during switching scenes after the build
        //DontDestroyOnLoad(gameObject);
    }

    // Add log: https://discussions.unity.com/t/i-want-to-create-an-in-game-log-which-will-print-actions-that-the-player-is-making/77995/2
    public void AddLogMessage(string message)
    {
        // Remove log messages if there are more than max lines
        if (logMessages.Count > maxLogLines)
        {
            logMessages.Dequeue();
        }
        // Add the message to the log queue
        logMessages.Enqueue(message);

        // Update log messages
        UpdatePlayerLog();
    }

    // Update player logs (when upgrades happen)
    private void UpdatePlayerLog()
    {
        // Combine all log messages 
        string logTextString = string.Join("\n ", logMessages);

        // Update text
        logText.text = logTextString;
    }

    // Add quest log messages (when boss dies and player can move to the next level)
    public void AddQuestLogMessage(string message)
    {
        // Remove log messages if there are more than max lines
        if (questLogMessages.Count > maxLogLines)
        {
            questLogMessages.Dequeue();
        }
        // Add the message to the log queue
        questLogMessages.Enqueue(message);

        // Update log messages
        UpdateQuestPlayerLog();
    }

    private void UpdateQuestPlayerLog()
    {
        // Combine all log messages 
        string logTextString = string.Join("\n--> ", questLogMessages);

        // Update text
        questText.text = logTextString;
    }
}
