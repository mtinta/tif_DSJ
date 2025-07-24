using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Puntaje
{
    public string nombre;
    public float tiempo;
}

[System.Serializable]
public class ListaPuntajes
{
    public List<Puntaje> puntajes = new List<Puntaje>();
}

public class PuntajeManager : MonoBehaviour
{
    public TMP_InputField inputNombre;            // Campo de entrada del nombre
    public GameObject prefabTextoPuntaje;         // Prefab de texto (TextMeshProUGUI)
    public Transform contenedorPuntajes;          // Contenedor vertical en UI
    public TextMeshProUGUI tiempoTexto;           // Texto que muestra "Tu tiempo: 00:00"

    private float tiempoActual;

    void Start()
    {
        // Obtener el tiempo guardado por el jugador
        tiempoActual = PlayerPrefs.GetFloat("tiempo_final", -1f);

        if (tiempoTexto != null)
        {
            tiempoTexto.text = tiempoActual >= 0 ? $"Tu tiempo: {FormatearTiempo(tiempoActual)}" : "Tiempo no disponible";
        }

        MostrarTop5(); // Mostrar los puntajes guardados
    }

    public void GuardarYMostrarPuntaje()
    {
        string nombre = inputNombre.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Nombre vacío. No se guardará el puntaje.");
            return;
        }

        if (tiempoActual < 0f)
        {
            Debug.LogWarning("No se encontró tiempo válido.");
            return;
        }

        // Cargar datos anteriores
        string json = PlayerPrefs.GetString("mejores_puntajes", "");
        ListaPuntajes lista = string.IsNullOrEmpty(json) ? new ListaPuntajes() : JsonUtility.FromJson<ListaPuntajes>(json);

        // Agregar nuevo puntaje
        lista.puntajes.Add(new Puntaje { nombre = nombre, tiempo = tiempoActual });

        // Ordenar y guardar los mejores 5
        lista.puntajes = lista.puntajes.OrderBy(p => p.tiempo).Take(5).ToList();

        // Guardar en PlayerPrefs
        string nuevoJson = JsonUtility.ToJson(lista);
        PlayerPrefs.SetString("mejores_puntajes", nuevoJson);
        PlayerPrefs.Save();

        Debug.Log("Puntaje guardado: " + nombre + " - " + tiempoActual);

        // Refrescar la lista
        foreach (Transform child in contenedorPuntajes)
            Destroy(child.gameObject);

        MostrarTop5();
    }

    void MostrarTop5()
    {
        string json = PlayerPrefs.GetString("mejores_puntajes", "");
        if (string.IsNullOrEmpty(json)) return;

        ListaPuntajes lista = JsonUtility.FromJson<ListaPuntajes>(json);

        foreach (var puntaje in lista.puntajes)
        {
            GameObject textoGO = Instantiate(prefabTextoPuntaje, contenedorPuntajes);
            TextMeshProUGUI tmp = textoGO.GetComponent<TextMeshProUGUI>();
            tmp.text = $"{puntaje.nombre} - {FormatearTiempo(puntaje.tiempo)}";
        }
    }

    string FormatearTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        return minutos > 0 ? $"{minutos}:{segundos:00}" : $"{segundos}s";
    }
}
