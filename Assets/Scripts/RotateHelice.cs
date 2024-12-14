using UnityEngine;

public class RotateHelice : MonoBehaviour
{
    public Transform targetHelice;
    public Vector3 pivotOffset; // Offset relativo a la hélice (local)
    public float rotationSpeed = 100f;
    private float currentSpeed = 0f;

    void Update()
    {
        if (targetHelice != null)
        {
            // Calcula el punto de pivote en coordenadas globales usando el offset local
            Vector3 pivotPoint = targetHelice.TransformPoint(pivotOffset);

            // Rota alrededor del punto de pivote usando el eje Y local de la hélice
            targetHelice.RotateAround(pivotPoint, targetHelice.up, currentSpeed * Time.deltaTime);
        }
    }

    public void RotateForward()
    {
        currentSpeed = rotationSpeed;
    }

    public void RotateBackward()
    {
        currentSpeed = -rotationSpeed;
    }

    public void StopRotation()
    {
        currentSpeed = 0f;
    }
}
