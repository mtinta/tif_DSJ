using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] private Button botonContinuar;    // Botón "Continuar Juego"
    [SerializeField] private Button botonEscoger;      // Botón "Escoger Mapa"
    [SerializeField] private Button botonSalir;        // Botón "Salir"

    private void Start()
    {
        if (botonContinuar != null)
            botonContinuar.onClick.AddListener(() => CargarEscena("escena_juego"));
        else
            Debug.LogWarning("Botón Continuar no asignado.");

        if (botonEscoger != null)
            botonEscoger.onClick.AddListener(() => CargarEscena("menu_stages"));
        else
            Debug.LogWarning("Botón Escoger no asignado.");

        if (botonSalir != null)
            botonSalir.onClick.AddListener(SalirAplicacion);
        else
            Debug.LogWarning("Botón Salir no asignado.");
    }

    private void CargarEscena(string nombreEscena)
    {
        if (Application.CanStreamedLevelBeLoaded(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogError($"La escena '{nombreEscena}' no está incluida en Build Settings.");
        }
    }

    private void SalirAplicacion()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("La aplicación se ha cerrado.");
    }
}
