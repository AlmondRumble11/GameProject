using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Player moving values
    public float playerMovementSpeed = 10f;
    private Vector2 playerMovementDirection;
    private Vector2 playerMousePosition;

    // Rigid body object of the player character
    public Rigidbody2D rigidBody;
    // Camera location
    public Camera mainCamera;

    // Weapon
    public PlayerWeaponScript playerWeaponScript;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Gets the player keyboard inputs
        ProcessPlayerInputs();
    }
    // Calculate player movement values (i.e physics calculations)
    // Calls after every input
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Player keyboard inputs
    void ProcessPlayerInputs()
    {
        // Get player movements
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var horizontalMovement = Input.GetAxisRaw("Horizontal");

        // Fire the weapon if mouse left click is pressed
        if (Input.GetMouseButtonDown(0))
        {
            playerWeaponScript.PlayerWeaponFire();
        }


        // Assing moving direction of the player
        // Normalized is used to keep z-axis velocity same as xy-axis velocity
        playerMovementDirection = new Vector2(horizontalMovement, verticalMovement).normalized;

        // Get mouse position (i.e where the mouse is in the main camera view)
        playerMousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
    }

    // Player movement
    void MovePlayer()
    {
        // Get the new velocity values
        var verticalVelocity = playerMovementDirection.x * playerMovementSpeed;
        var horizontalVelocity = playerMovementDirection.y * playerMovementSpeed;

        // Assing new velocity values
        rigidBody.velocity = new Vector2(verticalVelocity, horizontalVelocity);

        // Player rotation
        var playerAimDirection = playerMousePosition - rigidBody.position;
        var aimingAngle = Mathf.Atan2(playerAimDirection.y, playerAimDirection.x) * Mathf.Rad2Deg - 90f;
        rigidBody.rotation = aimingAngle;

    }
}
