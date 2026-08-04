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

    [Header("Sonido de proximidad de este portal")]
    [Tooltip("Componente ProximidadPortal ubicado en este mismo portal, si tiene sonido de acercamiento.")]
    public ProximidadPortal proximidadPortal;

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
        float duracionAudioEfectiva = duracionFadeMusica;

        if (usarFadeNegro && FadeManager.Instance != null)
            duracionAudioEfectiva = Mathf.Min(duracionFadeMusica, FadeManager.Instance.DuracionFade);

        if (proximidadPortal != null)
            proximidadPortal.ForzarFadeOut(duracionAudioEfectiva);

        if (musicaGeneral != null)
            StartCoroutine(FadeVolumenMusica(musicaGeneral, duracionAudioEfectiva));

        if (usarFadeNegro && FadeManager.Instance != null)
            FadeManager.Instance.CambiarEscena(nombreEscena);
        else
            SceneManager.LoadScene(nombreEscena);

        yield return null;
    }

    private IEnumerator FadeVolumenMusica(AudioSource source, float duracion)
    {
        float volumenInicial = source.volume;
        float t = 0f;

        while (t < duracion)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }

        source.volume = 0f;
    }
}