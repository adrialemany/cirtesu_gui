using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] vertices;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.mesh != null)
        {
            vertices = meshFilter.mesh.vertices;
        }
        else
        {
            Debug.LogError("No se encontró un Mesh en el MeshFilter.");
        }
    }

    private void Update()
    {
        if (vertices == null || vertices.Length == 0)
        {
            Debug.LogWarning("Los vértices del mesh no están inicializados.");
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            if (WaveManager.instance != null)
            {
                vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
            }
        }
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
