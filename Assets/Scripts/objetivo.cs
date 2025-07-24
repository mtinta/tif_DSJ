using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetivoFinal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Guardar tiempo como float con la clave correcta
            PlayerPrefs.SetFloat("tiempo_final", Timer.ElapsedTime);
            PlayerPrefs.Save();

            // Cargar escena de puntajes
            SceneManager.LoadScene("menu_puntajes");
        }
    }
}
