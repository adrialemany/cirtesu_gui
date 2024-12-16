using UnityEngine;
using UnityEngine.UI;

public class CanvasActivator : MonoBehaviour
{
    public Canvas canvasToActivate; // Canvas que se activará
    public Canvas canvasToDeactivateAtStart; // Canvas que se desactivará al inicio
    public Button targetButton; // Botón que se pulsará al presionar "E"
    public string playerTag = "Player"; // Tag del Player

    private bool isCanvasActive = false; // Bandera para saber si el Canvas está activo

    void Start()
    {
        // Desactivar el Canvas que se desactivará al inicio
        if (canvasToDeactivateAtStart != null)
        {
            canvasToDeactivateAtStart.gameObject.SetActive(false);
            Debug.Log("Canvas desactivado al inicio.");
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas para desactivar en el inicio.");
        }

        // Desactivar el Canvas a activar si es necesario
        if (canvasToActivate != null)
        {
            canvasToActivate.gameObject.SetActive(false); // Se desactiva al principio, si es necesario
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas para activar en el inspector.");
        }
    }

    void Update()
    {
        if (isCanvasActive && Input.GetKeyDown(KeyCode.E))
        {
            if (targetButton != null)
            {
                targetButton.onClick.Invoke();
            }
            else
            {
                Debug.LogWarning("No se asignó un botón en el inspector.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (canvasToActivate != null)
            {
                canvasToActivate.gameObject.SetActive(true);
                isCanvasActive = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (canvasToActivate != null)
            {
                canvasToActivate.gameObject.SetActive(false);
                isCanvasActive = false;
            }
        }
    }
}
