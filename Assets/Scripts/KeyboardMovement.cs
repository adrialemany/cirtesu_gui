using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public GameObject v;
    public float velocityX;
    public float velocityZ; // Forward/backward speed
    public float mouseSensitivity;

    public float horizontalInput;
    public float verticalInput;

    public float rotationX; // Rotation for the camera view (pitch)
    public float rotationY; // Rotation for the character view (yaw)

    public float pitchLimit = 80f; // Limit for looking up/down

    private KeyboardRobot keyboardRobot; // Referencia al script KeyboardRobot

    void Start()
    {
        velocityX = 5f; // Sideways movement speed
        velocityZ = 5f; // Forward/backward movement speed
        mouseSensitivity = 100f; // Adjust for rotation speed

        rotationX = 0;
        rotationY = transform.eulerAngles.y;

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Buscar y desactivar el script KeyboardRobot
        keyboardRobot = FindObjectOfType<KeyboardRobot>();
        if (keyboardRobot != null)
        {
            keyboardRobot.enabled = false;
        }
        else
        {
            Debug.LogWarning("No se encontró el script KeyboardRobot en la escena.");
        }
    }

    void Update()
    {
        // Get keyboard input for movement
        horizontalInput = Input.GetAxis("Horizontal"); // A/D for left/right movement
        verticalInput = Input.GetAxis("Vertical");     // W/S for forward/backward movement

        // Move forward/backward with 'W' and 'S', and left/right with 'A' and 'D'
        Vector3 movement = new Vector3(horizontalInput * velocityX * Time.deltaTime, 0, verticalInput * velocityZ * Time.deltaTime);
        transform.Translate(movement, Space.Self);

        // Get mouse input for rotation (relative movement)
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");

        // Rotate around the Y-axis (Yaw) for looking left/right
        rotationY += mouseInputX * mouseSensitivity * Time.deltaTime;

        // Rotate around the X-axis (Pitch) for looking up/down
        rotationX -= mouseInputY * mouseSensitivity * Time.deltaTime;

        // Clamp the pitch rotation to avoid looking too far up or down
        rotationX = Mathf.Clamp(rotationX, -pitchLimit, pitchLimit);

        // Apply the rotations to the character and camera
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}
