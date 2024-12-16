using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public GameObject v;
    public float velocityX;
    public float velocityZ;
    public float mouseSensitivity;

    public float horizontalInput;
    public float verticalInput;

    public float rotationX;
    public float rotationY;

    public float pitchLimit = 80f;
    private KeyboardRobot keyboardRobot;

    void Start()
    {
        velocityX = 5f;
        velocityZ = 5f;
        mouseSensitivity = 100f;

        rotationX = 0;
        rotationY = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput * velocityX * Time.deltaTime, 0, verticalInput * velocityZ * Time.deltaTime);
        transform.Translate(movement, Space.Self);

        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");

        rotationY += mouseInputX * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseInputY * mouseSensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -pitchLimit, pitchLimit);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}
