using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalScript : MonoBehaviour
{
    //  Store game object
    public GameObject portalStore;

    // Store values 
    public List<NormalUpgrade> upgrades;
    public UpgradeManagerScript upgradeManagerScript;
    public GameLogicManagerScript gameLogicManager;

    // Buttons
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;

    // Audio
    public AudioSource audioSource;

    public void Start()
    {
        // Get sciprts
        upgradeManagerScript = FindObjectOfType<UpgradeManagerScript>();
        gameLogicManager = GameObject.FindGameObjectWithTag("GameLogicManager").GetComponent<GameLogicManagerScript>();

        // Get upgrades
        upgrades = upgradeManagerScript.portalStoreUpgrades;

        // Assing listeners and add text
        option1Button.onClick.AddListener(OnOption1Click);
        option2Button.onClick.AddListener(OnOption2Click);
        option3Button.onClick.AddListener(OnOption3Click);
        AssignButtonText();

    }

    // Add text 
    private void AssignButtonText()
    {
        option1Button.GetComponentInChildren<Text>().text = $"{upgrades[0].Name}: {upgrades[0].Price}";
        option2Button.GetComponentInChildren<Text>().text = $"{upgrades[1].Name}: {upgrades[1].Price}";
        option3Button.GetComponentInChildren<Text>().text = $"{upgrades[2].Name}: {upgrades[2].Price}";
    }


    // When player is in the zone --> activate the store
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Show the selection UI
            portalStore.SetActive(true);
        }
    }

    // Close the store when player leaves
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hide the selection UI and reset the selected item
            portalStore.SetActive(false);

        }
    }

    // Check if enough demon hearts
    public bool HasEnoughHearts(int price)
    {
        var score = GameData.score;
        return score >= price;
    }

    // Health
    public void OnOption1Click()
    {
        var selectedItem = upgrades[0];
        HandleItemClick(selectedItem);
    }

    // Speed
    public void OnOption2Click()
    {
        var selectedItem = upgrades[1];
        HandleItemClick(selectedItem);
    }

    // Damage
    public void OnOption3Click()
    {
        var selectedItem = upgrades[2];
        HandleItemClick(selectedItem);
    }

    private void HandleItemClick(NormalUpgrade selectedItem)
    {
        if (HasEnoughHearts(selectedItem.Price))
        {
            // Assing new upgrade and remove score
            var playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            playerScript.AssingUpgrade(selectedItem);
            gameLogicManager.RemoveScore(selectedItem.Price);
            audioSource.Play();
        }
        else
        {
            gameLogicManager.AddPlayerActionLog($"Not enough demon hearts collected");
        }
    }
}

