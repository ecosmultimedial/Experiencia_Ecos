using UnityEngine;

public class TriggerCanvaUnaVez : MonoBehaviour
{
    public GameObject canvas;
    public float duracion = 6f; // Segundos que se muestra el canvas

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
            Invoke("OcultarCanvas", duracion);
        }
    }

    void OcultarCanvas()
    {
        canvas.SetActive(false);
    }
}
