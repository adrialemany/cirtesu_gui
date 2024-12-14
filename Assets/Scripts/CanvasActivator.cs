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
        // Detectar si la tecla "E" se presiona y el Canvas está activo
        if (isCanvasActive && Input.GetKeyDown(KeyCode.E))
        {
            if (targetButton != null)
            {
                // Simular clic en el botón
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
        // Verificar si el objeto que entra en el trigger tiene el tag del Player
        if (other.CompareTag(playerTag))
        {
            // Activar el Canvas
            if (canvasToActivate != null)
            {
                canvasToActivate.gameObject.SetActive(true);
                isCanvasActive = true; // Activar la bandera
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Desactivar el Canvas y la bandera al salir del trigger
        if (other.CompareTag(playerTag))
        {
            if (canvasToActivate != null)
            {
                canvasToActivate.gameObject.SetActive(false);
                isCanvasActive = false; // Desactivar la bandera
            }
        }
    }
}
