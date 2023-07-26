using UnityEngine;

public class DemonHeartScript : MonoBehaviour
{
    // Game logic manager 
    public GameLogicManagerScript gameLogicManager;

    // Audio
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        gameLogicManager = GameObject.FindGameObjectWithTag("GameLogicManager").GetComponent<GameLogicManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Add score, play sound and destroy the object
            gameLogicManager.AddDemonHeart(1);
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            Destroy(gameObject);
        }
    }
}
