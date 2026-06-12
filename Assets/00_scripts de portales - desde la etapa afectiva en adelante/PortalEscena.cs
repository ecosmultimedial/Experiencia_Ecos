using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEscena : MonoBehaviour
{
    public string nombreEscena = "etapa central";
    public bool usarFadeNegro = true;

    [Header("Identificación de origen")]
    [Tooltip("Nombre de la escena donde está ESTE portal.")]
    public string identificadorOrigen = "";

    [Header("Marcar etapa como completada")]
    [Tooltip("Si esta marcado, marca la etapa con el nombre de identificadorOrigen como completada antes del fade.")]
    public bool marcarEtapaComoCompletada = false;

    [Header("Zocalo")]
    public ZocaloTypewriter zocalo;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(identificadorOrigen))
            {
                PlayerPrefs.SetString("OrigenEscena", identificadorOrigen);
                if (marcarEtapaComoCompletada)
                {
                    PlayerPrefs.SetInt("Etapa" + identificadorOrigen + "Completada", 1);
                }
                PlayerPrefs.Save();
            }

            if (zocalo != null) zocalo.Desactivar();

            if (usarFadeNegro && FadeManager.Instance != null)
                FadeManager.Instance.CambiarEscena(nombreEscena);
            else
                SceneManager.LoadScene(nombreEscena);
        }
    }
}