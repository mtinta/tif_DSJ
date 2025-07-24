using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class FigurasDesdeJSON : MonoBehaviour
{
    [System.Serializable]
    public class Punto
    {
        public float x;
        public float y;
    }

    public float escala = 0.01f;
    public Vector2 offset = new Vector2(-10f, 5f);
    public Camera mainCamera;
    public SpriteRenderer fondoRenderer; // Asigna en el inspector el objeto "Fondo"

    void Start()
    {
        string nombreArchivo = PlayerPrefs.GetString("archivoSeleccionado", "");
        if (string.IsNullOrEmpty(nombreArchivo))
        {
            Debug.LogError("No se ha seleccionado ningún archivo JSON.");
            return;
        }

        TextAsset jsonFile = Resources.Load<TextAsset>("Escenarios/" + nombreArchivo);
        if (jsonFile == null)
        {
            Debug.LogError("No se pudo cargar el archivo JSON: " + nombreArchivo);
            return;
        }

        var formas = JsonConvert.DeserializeObject<Dictionary<string, List<Punto>>>(jsonFile.text);

        int contador = 1;
        foreach (var forma in formas)
        {
            var puntos = ConvertirAPuntosEscalados(forma.Value);
            CrearFigura(puntos, "Forma " + contador);
            contador++;
        }

        // Cargar la imagen con el mismo nombre que el JSON
        Sprite imagenFondo = Resources.Load<Sprite>("Escenarios/" + nombreArchivo);
        if (imagenFondo != null && fondoRenderer != null)
        {
            fondoRenderer.sprite = imagenFondo;

            // Tamaño en píxeles
            float anchoPx = imagenFondo.texture.width;
            float altoPx = imagenFondo.texture.height;

            // Escala de Unity (basada en Pixels Per Unit del sprite importado)
            float escalaPPU = 0.5f / imagenFondo.pixelsPerUnit;

            // Escalamos para que se vea con su tamaño real
            fondoRenderer.transform.localScale = new Vector3(
                anchoPx * escalaPPU,
                altoPx * escalaPPU,
                1f
            );
            fondoRenderer.transform.localScale = Vector3.one;

            // Posicionar el fondo alineado con las figuras (como si empezaran en 0,0)
            fondoRenderer.transform.position = new Vector3(
            10f + offset.x,
            -5f + offset.y,
            10f
            

);  
        }
        else
        {
            Debug.LogWarning("No se pudo cargar la imagen de fondo o el SpriteRenderer no está asignado.");
        }

        if (mainCamera == null)
            mainCamera = Camera.main;
    }


    List<Vector2> ConvertirAPuntosEscalados(List<Punto> puntosOriginales)
    {
        var lista = new List<Vector2>();
        foreach (var p in puntosOriginales)
        {
            lista.Add(new Vector2(
                p.x * escala + offset.x,
                -p.y * escala + offset.y // Invertir eje Y
            ));
        }
        return lista;
    }

    void CrearFigura(List<Vector2> puntos, string nombre)
    {
        GameObject figura = new GameObject(nombre);
        figura.transform.position = mainCamera.transform.position;
        figura.layer = LayerMask.NameToLayer("Ground");

        MeshFilter meshFilter = figura.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = figura.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[puntos.Count];
        for (int i = 0; i < puntos.Count; i++)
            vertices[i] = new Vector3(puntos[i].x, puntos[i].y, 0);

        mesh.vertices = vertices;
        mesh.triangles = Triangulate(puntos);

        Rigidbody2D rb2d = figura.AddComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Static;

        PolygonCollider2D collider = figura.AddComponent<PolygonCollider2D>();
        collider.points = puntos.ToArray();
    }

    int[] Triangulate(List<Vector2> puntos)
    {
        List<int> triangles = new List<int>();
        for (int i = 1; i < puntos.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        return triangles.ToArray();
    }
}
