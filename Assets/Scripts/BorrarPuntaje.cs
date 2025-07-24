using UnityEngine;

public class BorrarPuntajes : MonoBehaviour
{
    public void BorrarTodo()
    {
        PlayerPrefs.DeleteKey("mejores_puntajes");
        PlayerPrefs.DeleteKey("tiempoFinal");
        PlayerPrefs.DeleteKey("tiempoFinalFloat");
        PlayerPrefs.Save();
        Debug.Log("¡Puntajes eliminados!");
    }
}
