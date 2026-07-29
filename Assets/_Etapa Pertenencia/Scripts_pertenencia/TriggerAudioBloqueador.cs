using UnityEngine;

public class TriggerAudioBloqueador : MonoBehaviour
{
    public GameObject paredBloqueadora; // La pared invisible que bloquea
    public AudioSource audioSource; // El audio a reproducir
    private bool yaActivado = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;

            // Reproducir el audio
            audioSource.Play();

            // Esperar a que termine el audio y luego desbloquear
            Invoke("DesbloqueoPaso", audioSource.clip.length);
        }
    }

    void DesbloqueoPaso()
    {
        paredBloqueadora.SetActive(false);
    }
}