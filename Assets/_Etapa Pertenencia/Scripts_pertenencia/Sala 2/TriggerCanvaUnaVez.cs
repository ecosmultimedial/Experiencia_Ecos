using UnityEngine;

public class TriggerCanvaUnaVez : MonoBehaviour
{
    public GameObject canvas;
    public AudioSource audioNarracion; // Referencia al audio
    public float duracion = 9f; // Ahora 7 segundos para coincidir con el audio
    private bool yaActivado = false;

    void Start()
    {
        canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            canvas.SetActive(true);

            // Reproducir el audio con delay de 0.5 segundos
            Invoke("ReproducirAudio", 0.5f);

            Invoke("OcultarCanvas", duracion);
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
