using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarJuego : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Menu_principal");
        }
    }
}
