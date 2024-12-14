using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float depthBeforesubmerged = 1f;
    public float displacementAmount = 3f;

    private void Awake()
    {
        // Automatically assign the Rigidbody if not set in the Inspector
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforesubmerged) * displacementAmount;
            rigidbody.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
        }
    }
}
