using System.Collections.Generic;
using UnityEngine;

public class GenerarObjetosDesdeJSON : MonoBehaviour
{
    [System.Serializable]
    public class Formas
    {
        public Dictionary<string, List<Vector2>> formas;
    }

    public GameObject prefabBase; // Prefab que tendrá RigidBody2D y PolygonCollider2D
    public float escala = 0.01f;  // Escala para adaptar las figuras al mundo Unity

    void Start()
    {
        // Leer el nombre del archivo seleccionado desde PlayerPrefs
        string nombreArchivo = PlayerPrefs.GetString("archivoSeleccionado", "");

        if (string.IsNullOrEmpty(nombreArchivo))
        {
            Debug.LogError("No se ha seleccionado ningún archivo JSON.");
            return;
        }

        // Cargar el JSON desde Resources/Escenarios/<nombre>.json
        TextAsset jsonFile = Resources.Load<TextAsset>("Escenarios/" + nombreArchivo);

        if (jsonFile == null)
        {
            Debug.LogError("No se pudo cargar el archivo JSON: " + nombreArchivo);
            return;
        }

        if (prefabBase == null)
        {
            Debug.LogError("Por favor asigna el prefab base.");
            return;
        }

        // Deserializar el texto JSON en el diccionario de formas
        Formas formas = JsonUtility.FromJson<Formas>("{\"formas\":" + jsonFile.text + "}");
        if (formas.formas == null)
        {
            Debug.LogError("No se pudieron cargar las formas del archivo JSON.");
            return;
        }

        // Crear un objeto por cada forma en el JSON
        foreach (var forma in formas.formas)
        {
            CrearObjeto(forma.Value);
        }
    }

    void CrearObjeto(List<Vector2> puntos)
    {
        Vector2[] puntosEscalados = new Vector2[puntos.Count];
        for (int i = 0; i < puntos.Count; i++)
        {
            puntosEscalados[i] = new Vector2(
                puntos[i].x * escala,
                puntos[i].y * escala * -1  // Invertir eje Y para coordenadas de Unity
            );
        }

        // Calcular el centroide del polígono para posicionarlo correctamente
        Vector2 centro = CalcularCentro(puntosEscalados);

        GameObject objeto = Instantiate(prefabBase, centro, Quaternion.identity);
        objeto.name = "Forma";
        objeto.layer = LayerMask.NameToLayer("Ground");

        PolygonCollider2D collider = objeto.GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
            collider.points = puntosEscalados;
        }
        else
        {
            Debug.LogWarning("El prefab no tiene PolygonCollider2D.");
        }
    }

    Vector2 CalcularCentro(Vector2[] puntos)
    {
        Vector2 centro = Vector2.zero;
        foreach (var punto in puntos)
        {
            centro += punto;
        }
        return centro / puntos.Length;
    }
}
