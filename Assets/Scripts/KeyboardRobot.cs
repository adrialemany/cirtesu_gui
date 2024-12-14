using UnityEngine;

public class KeyboardRobot : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private KeyboardMovement keyboardMovementScript;
    private CameraSwitcher cameraSwitcherScript;

    public Canvas canvasToActivate;
    public Canvas canvasToDestroy;

    private Rigidbody rb;

    void Start()
    {
        keyboardMovementScript = FindObjectOfType<KeyboardMovement>();
        if (keyboardMovementScript != null)
        {
            keyboardMovementScript.enabled = false;
            Debug.Log("El script KeyboardMovement ha sido desactivado.");
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("El bloqueo del ratón ha sido desactivado.");
        }
        else
        {
            Debug.LogWarning("No se encontró el script KeyboardMovement en la escena.");
        }

        cameraSwitcherScript = FindObjectOfType<CameraSwitcher>();
        if (cameraSwitcherScript != null)
        {
            cameraSwitcherScript.enabled = false;
            Debug.Log("El script CameraSwitcher ha sido desactivado.");
        }
        else
        {
            Debug.LogWarning("No se encontró el script CameraSwitcher en la escena.");
        }

        if (canvasToActivate != null)
        {
            canvasToActivate.gameObject.SetActive(true);
            Debug.Log("El Canvas ha sido activado.");
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas para activar en el inspector.");
        }

        if (canvasToDestroy != null)
        {
            Destroy(canvasToDestroy.gameObject);
            Debug.Log("El Canvas ha sido eliminado.");
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas para eliminar en el inspector.");
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un componente Rigidbody en el objeto.");
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

}
