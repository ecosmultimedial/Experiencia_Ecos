using System.Collections;
using UnityEngine;

public class ProximityGlow : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource vozInstrucciones; // La voz en off con las instrucciones
    public float delayAntesDeVoz = 0.5f;   // Espera 1 segundo antes de reproducir

    [Header("Evento")]
    public CubeEventManager eventManager;

    private bool yaActivado = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(ReproducirVozYActivarEvento());
        }
    }

    IEnumerator ReproducirVozYActivarEvento()
    {
        // Esperar 1 segundo antes de la voz
        yield return new WaitForSeconds(delayAntesDeVoz);

        // Reproducir la voz de instrucciones
        if (vozInstrucciones != null)
            vozInstrucciones.Play();

        // Esperar a que termine la voz antes de activar el evento
        float duracionVoz = vozInstrucciones != null ? vozInstrucciones.clip.length : 0f;
        yield return new WaitForSeconds(duracionVoz);

        // Activar el evento que mostrará el botón continuar
        if (eventManager != null)
            eventManager.StartEvent();
    }
}