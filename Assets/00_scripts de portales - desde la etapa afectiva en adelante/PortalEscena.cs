using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEscena : MonoBehaviour
{
    public string nombreEscena = "etapa central";
    public bool usarFadeNegro = true;   // Desmarcar en el portal que va a la escena final

    [Header("Identificación de origen")]
    [Tooltip("Nombre de la escena donde está ESTE portal. La escena destino lo usa para saber de dónde viene el player.")]
    public string identificadorOrigen = "";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Guardar de dónde viene el player antes de cambiar de escena
            if (!string.IsNullOrEmpty(identificadorOrigen))
            {
                PlayerPrefs.SetString("OrigenEscena", identificadorOrigen);
                PlayerPrefs.Save();
            }

            if (usarFadeNegro && FadeManager.Instance != null)
                FadeManager.Instance.CambiarEscena(nombreEscena);
            else
                SceneManager.LoadScene(nombreEscena);
        }
    }
}
