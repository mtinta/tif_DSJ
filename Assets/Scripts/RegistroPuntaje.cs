using TMPro;
using UnityEngine;

public class RegistroPuntaje : MonoBehaviour
{
    public TextMeshProUGUI tiempoTexto;
    public TMP_InputField inputNombre;
    public TextMeshProUGUI mensajeConfirmacion;

    void Start()
    {
        string tiempo = PlayerPrefs.GetString("tiempoFinal", "00:00");
        tiempoTexto.text = "Tu tiempo: " + tiempo;
    }

    public void GuardarPuntaje()
    {
        string nombre = inputNombre.text;
        string tiempo = PlayerPrefs.GetString("tiempoFinal", "00:00");

        if (!string.IsNullOrEmpty(nombre))
        {
            PlayerPrefs.SetString("ultimoNombre", nombre);
            PlayerPrefs.SetString("ultimoTiempo", tiempo);
            PlayerPrefs.Save();

            mensajeConfirmacion.text = "Puntaje guardado ";
        }
        else
        {
            mensajeConfirmacion.text = "Por favor ingresa un nombre.";
        }
    }
}
