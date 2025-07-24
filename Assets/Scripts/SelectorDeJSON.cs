using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectorDeJSON : MonoBehaviour
{
    public Transform contenedorBotones;
    public GameObject botonPrefab;
    private const string ruta = "Escenarios";

    void Start()
    {
        Sprite[] imagenes = Resources.LoadAll<Sprite>(ruta);

        foreach (var imagen in imagenes)
        {
            string nombreBase = imagen.name;
            TextAsset json = Resources.Load<TextAsset>($"{ruta}/{nombreBase}");

            if (json == null)
                continue;

            GameObject btn = Instantiate(botonPrefab, contenedorBotones);
            Image img = btn.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = imagen;
                img.preserveAspect = true;
            }

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("archivoSeleccionado", nombreBase);
                SceneManager.LoadScene("escena_juego");
            });
        }
    }
}
