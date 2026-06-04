using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEscena : MonoBehaviour
{
    public string nombreEscena = "etapa central";
    public bool usarFadeNegro = true;   // Desmarcar en el portal que va a la escena final

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (usarFadeNegro && FadeManager.Instance != null)
                FadeManager.Instance.CambiarEscena(nombreEscena);
            else
                SceneManager.LoadScene(nombreEscena);
        }
    }
}