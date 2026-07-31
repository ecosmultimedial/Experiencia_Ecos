using UnityEngine;

public class TriggerCanvaUnaVez : MonoBehaviour
{
    public GameObject canvas;
    public AudioSource audioNarracion; // Referencia al audio
    public float duracion = 9f;
    private bool yaActivado = false;

    void Start()
    {
        canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // El Canvas solo se muestra la primera vez
            if (!yaActivado)
            {
                yaActivado = true;
                canvas.SetActive(true);
                Invoke("OcultarCanvas", duracion);
            }

            // El audio se reproduce/reinicia CADA VEZ que entra
            if (audioNarracion != null)
            {
                audioNarracion.Stop(); // Detener para reiniciar desde el inicio
                Invoke("ReproducirAudio", 0.5f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cancelar los Invoke pendientes
            CancelInvoke("ReproducirAudio");
            // El audio sigue sonando mientras está afuera
        }
    }

    void ReproducirAudio()
    {
        if (audioNarracion != null)
        {
            audioNarracion.Play();
        }
    }

    void OcultarCanvas()
    {
        canvas.SetActive(false);
    }
}