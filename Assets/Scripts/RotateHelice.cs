using UnityEngine;

public class RotateHelice : MonoBehaviour
{
    public Transform targetHelice;
    public Vector3 pivotOffset;
    public float rotationSpeed = 100f;
    private float currentSpeed = 0f;

    void Update()
    {
        if (targetHelice != null)
        {
            Vector3 pivotPoint = targetHelice.TransformPoint(pivotOffset);

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
