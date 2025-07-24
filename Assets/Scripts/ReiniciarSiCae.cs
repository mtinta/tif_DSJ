using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarSiCae : MonoBehaviour
{
    public float limiteInferior = -10f; // Puedes ajustar este valor seg�n tu escena

    void Update()
    {
        if (transform.position.y < limiteInferior)
        {
            SceneManager.LoadScene("escena_juego");
        }
    }
}