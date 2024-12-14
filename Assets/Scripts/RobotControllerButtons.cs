using System.Collections;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

/// <summary>
/// Control del modelo 3D, publicación de velocidades al robot real y movimientos mediante botones
/// </summary>
public class RobotControllerButtons : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float smoothTransition = 2f;
    public float stepChange = 2f;

    private float currentMoveSpeed = 0f;
    private float currentRotationSpeed = 0f;

    private float targetMoveSpeed = 0f;
    private float targetRotationSpeed = 0f;

    private bool isChangingMoveDirection = false;
    private bool isChangingRotationDirection = false;

    private ROSConnection ros;
    public string topicName = "/cmd_vel";
    private TwistMsg twistMsg;

    public MonoBehaviour floaterScript;
    private Rigidbody rb;


    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topicName);

        twistMsg = new TwistMsg();

        Debug.Log("Conexión ROS inicializada y publicador registrado.");

        // Asignar el Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un componente Rigidbody en el objeto. Asegúrate de que el Rigidbody está agregado al GameObject.");
        }

        if (floaterScript != null)
        {
            floaterScript.enabled = true;
            Debug.Log("Script Floater activado.");
        }
        else
        {
            Debug.LogWarning("No se asignó FloaterScript en el inspector.");
        }
    }

    void Update()
    {
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, Time.deltaTime * smoothTransition);
        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, targetRotationSpeed, Time.deltaTime * smoothTransition);

        if (Mathf.Abs(currentMoveSpeed) > 0.01f)
        {
            Vector3 movement = transform.forward * currentMoveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }

        if (Mathf.Abs(currentRotationSpeed) > 0.01f)
        {
            float rotation = currentRotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }
    }

    public void MoveForward()
    {
        Debug.Log("Botón 'Avanzar' presionado.");
        if (currentMoveSpeed < 0 && !isChangingMoveDirection)
        {
            targetMoveSpeed = 0f;
            isChangingMoveDirection = true;
        }
        else if (isChangingMoveDirection)
        {
            targetMoveSpeed = moveSpeed;
            isChangingMoveDirection = false;
        }
        else
        {
            targetMoveSpeed = Mathf.Min(targetMoveSpeed + stepChange, moveSpeed);
        }

        PublishToROS();
    }

    public void MoveBackward()
    {
        Debug.Log("Botón 'Retroceder' presionado.");
        if (currentMoveSpeed > 0 && !isChangingMoveDirection)
        {
            targetMoveSpeed = 0f;
            isChangingMoveDirection = true;
        }
        else if (isChangingMoveDirection)
        {
            targetMoveSpeed = -moveSpeed;
            isChangingMoveDirection = false;
        }
        else
        {
            targetMoveSpeed = Mathf.Max(targetMoveSpeed - stepChange, -moveSpeed);
        }

        PublishToROS();
    }

    public void RotateRight()
    {
        Debug.Log("Botón 'Girar Derecha' presionado.");
        if (currentRotationSpeed < 0 && !isChangingRotationDirection)
        {
            targetRotationSpeed = 0f;
            isChangingRotationDirection = true;
        }
        else if (isChangingRotationDirection)
        {
            targetRotationSpeed = rotationSpeed;
            isChangingRotationDirection = false;
        }
        else
        {
            targetRotationSpeed = Mathf.Min(targetRotationSpeed + stepChange, rotationSpeed);
        }

        PublishToROS();
    }

    public void RotateLeft()
    {
        Debug.Log("Botón 'Girar Izquierda' presionado.");
        if (currentRotationSpeed > 0 && !isChangingRotationDirection)
        {
            targetRotationSpeed = 0f;
            isChangingRotationDirection = true;
        }
        else if (isChangingRotationDirection)
        {
            targetRotationSpeed = -rotationSpeed;
            isChangingRotationDirection = false;
        }
        else
        {
            targetRotationSpeed = Mathf.Max(targetRotationSpeed - stepChange, -rotationSpeed);
        }

        PublishToROS();
    }

    public void StopMoving()
    {
        Debug.Log("Botón 'Detener' presionado.");
        targetMoveSpeed = 0f;
        currentMoveSpeed = 0f;
        targetRotationSpeed = 0f;
        currentRotationSpeed = 0f;
        isChangingMoveDirection = false;
        isChangingRotationDirection = false;

        Debug.Log("Movimiento detenido.");

        PublishToROS();
    }

    public void StopRotating()
    {
        targetRotationSpeed = 0f;
        isChangingRotationDirection = false;
    }

    public void Relocate()
    {
        if (floaterScript != null)
        {
            floaterScript.enabled = false;
            Debug.Log("Script Floater desactivado.");
        }

        
        // Desactiva las físicas temporalmente
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep(); // Garantiza que no haya fuerzas residuales
        Debug.Log("Física detenida.");
        

        // Guardar la rotación actual del eje Y
        float currentYRotation = transform.rotation.eulerAngles.y;

        // Restablece la posición y mantiene la rotación actual del eje Y
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, -0.421f, currentPosition.z);
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

        // Restablece las velocidades y estados internos
        targetMoveSpeed = 0f;
        currentMoveSpeed = 0f;
        targetRotationSpeed = 0f;
        currentRotationSpeed = 0f;
        isChangingMoveDirection = false;
        isChangingRotationDirection = false;

        if (rb != null)
        {
            // Reactiva las físicas
            rb.isKinematic = false;
            rb.WakeUp(); // Garantiza que el Rigidbody vuelva a estar activo sin inercia
            Debug.Log("Física reactivada.");
        }

        if (floaterScript != null)
        {
            floaterScript.enabled = true;
            Debug.Log("Script Floater activado.");
        }

        Debug.Log("Robot recolocado: movimiento detenido, orientación restablecida y físicas reactivadas.");
    }

    private void PublishToROS()
    {
        if (ros == null)
        {
            Debug.LogWarning("ROSConnection no está inicializado.");
            return;
        }

        twistMsg.linear = new Vector3Msg(currentMoveSpeed, 0, 0);
        twistMsg.angular = new Vector3Msg(0, 0, currentRotationSpeed);

        ros.Publish(topicName, twistMsg);

        Debug.Log($"Publicado a ROS: linear={twistMsg.linear.x}, angular={twistMsg.angular.z}");
    }
}
