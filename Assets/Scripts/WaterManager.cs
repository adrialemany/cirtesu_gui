using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] vertices; // Variable para almacenar los vértices del mesh

    private void Awake()
    {
        // Obtener el componente MeshFilter en el GameObject
        meshFilter = GetComponent<MeshFilter>();

        // Asegurarnos de que el mesh no sea null
        if (meshFilter != null && meshFilter.mesh != null)
        {
            // Inicializar el array de vértices
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

        // Iterar sobre los vértices para aplicar las alturas de la ola
        for (int i = 0; i < vertices.Length; i++)
        {
            // Asumimos que WaveManager tiene un método llamado GetWaveHeight
            // que recibe un valor float y devuelve un Vector3.
            if (WaveManager.instance != null)
            {
                vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
            }
        }

        // Asignar los vértices actualizados al mesh
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
