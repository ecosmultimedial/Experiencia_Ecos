using System.Collections;
using UnityEngine;

public class ProximityGlow : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource vozInstrucciones;
    public float delayAntesDeVoz = 0.5f;
    public float duracionFadeOut = 1f;

    [Header("Evento")]
    public CubeEventManager eventManager;

    private bool eventoActivado = false;
    private Coroutine corrutinaActual;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !eventoActivado) // Agregar !eventoActivado
        {
            if (corrutinaActual != null)
                StopCoroutine(corrutinaActual);

            corrutinaActual = StartCoroutine(ReproducirVoz());
        }
    }

    // Al salir NO se corta — el audio sigue sonando
    // Solo se cancela la corrutina de espera para no activar el evento dos veces
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !eventoActivado) // Agregar !eventoActivado
        {
            if (corrutinaActual != null)
            {
                StopCoroutine(corrutinaActual);
                corrutinaActual = null;
            }
        }
    }

    IEnumerator ReproducirVoz()
    {
        // Si ya está sonando, la reiniciamos
        if (vozInstrucciones != null)
        {
            vozInstrucciones.Stop();
            vozInstrucciones.volume = 1f;
            yield return new WaitForSeconds(delayAntesDeVoz);
            vozInstrucciones.Play();
        }

        // Solo activar el evento una vez, cuando termine la voz
        if (!eventoActivado)
        {
            float duracionVoz = vozInstrucciones != null ? vozInstrucciones.clip.length : 0f;
            yield return new WaitForSeconds(duracionVoz);
            eventoActivado = true;
            if (eventManager != null)
                eventManager.StartEvent();
        }
    }

    // Este método lo llama el CubeEventManager cuando el player aprieta Enter
    public void FadeOutVoz()
    {
        if (vozInstrucciones != null && vozInstrucciones.isPlaying)
            StartCoroutine(HacerFadeOut());
    }

    IEnumerator HacerFadeOut()
    {
        float volumenInicial = vozInstrucciones.volume;
        float t = 0f;

        while (t < duracionFadeOut)
        {
            t += Time.deltaTime;
            vozInstrucciones.volume = Mathf.Lerp(volumenInicial, 0f, t / duracionFadeOut);
            yield return null;
        }

        vozInstrucciones.Stop();
        vozInstrucciones.volume = 1f;
    }
}