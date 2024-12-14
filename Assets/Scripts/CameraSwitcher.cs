using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // Referencia a la cámara inicial
    public Camera mainCamera1; // Referencia a la cámara que se activará

    private KeyboardRobot keyboardRobot; // Referencia al script KeyboardRobot

    void Start()
    {
        // Asegurarse de que la cámara inicial está activa y la otra desactivada
        mainCamera.gameObject.SetActive(true);
        mainCamera1.gameObject.SetActive(false);

        // Buscar y desactivar el script KeyboardRobot al inicio
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

    public void SwitchCamera()
    {
        // Cambiar entre las cámaras
        if (mainCamera.gameObject.activeSelf)
        {
            mainCamera.gameObject.SetActive(false);
            mainCamera1.gameObject.SetActive(true);

            // Activar el script KeyboardRobot
            if (keyboardRobot != null)
            {
                keyboardRobot.enabled = true;
            }
        }
        else
        {
            mainCamera.gameObject.SetActive(true);
            mainCamera1.gameObject.SetActive(false);

            // Desactivar el script KeyboardRobot
            if (keyboardRobot != null)
            {
                keyboardRobot.enabled = false;
            }
        }
    }
}
