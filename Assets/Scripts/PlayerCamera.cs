using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerCharacter; // Player character (i.e target)
    public Vector3 playerOffset = new Vector3(0f, 0f, -10f); // offset of the camera to the player
    public float damping = 10f; // Time it takes for the camera to react the target

    private void Update()
    {

        if (playerCharacter != null)
        {
            // New player position
            var playerMovePosition = playerCharacter.position + playerOffset;
            // Set the new camera position based on the player position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, playerMovePosition, damping * Time.deltaTime);
            transform.position = smoothedPosition;

        }
    }
}
