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

    [Header("Musica ambiente de la Central")]
    [Tooltip("AudioSource de la musica general (objeto 'Musica' en la escena Central).")]
    public AudioSource musicaGeneral;
    public float duracionFadeMusica = 2f;

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

            StartCoroutine(FadeMusicaYCambiarEscena());
        }
    }

    private IEnumerator FadeMusicaYCambiarEscena()
    {
        if (musicaGeneral != null)
        {
            float volumenInicial = musicaGeneral.volume;
            float t = 0f;

            while (t < duracionFadeMusica)
            {
                t += Time.deltaTime;
                musicaGeneral.volume = Mathf.Lerp(volumenInicial, 0f, t / duracionFadeMusica);
                yield return null;
            }

            musicaGeneral.volume = 0f;
        }

        if (usarFadeNegro && FadeManager.Instance != null)
            FadeManager.Instance.CambiarEscena(nombreEscena);
        else
            SceneManager.LoadScene(nombreEscena);
    }
}