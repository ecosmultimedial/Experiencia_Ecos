using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class TriggerVideo : MonoBehaviour
{
    public GameObject videoUI;
    public VideoPlayer videoPlayer;
    public Renderer esferaRenderer;

    [Header("Colores")]
    public Color colorVisto = new Color(0.41f, 0.91f, 0.92f); // celeste equivalente al amarillo
    public float duracionTransicion = 1.5f;

    private bool fueVista = false;
    private Color colorInicialBase;
    private Color colorInicialEmission;
    private Material materialInstancia;
    private Coroutine transicionActual;

    void Start()
    {
        videoUI.SetActive(false);
        materialInstancia = esferaRenderer.material;
        colorInicialBase = materialInstancia.GetColor("_BaseColor");
        colorInicialEmission = materialInstancia.GetColor("_EmissionColor");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            videoUI.SetActive(true);
            videoPlayer.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            videoPlayer.Stop();
            videoUI.SetActive(false);

            if (!fueVista)
            {
                fueVista = true;
                if (transicionActual != null)
                    StopCoroutine(transicionActual);
                transicionActual = StartCoroutine(CambiarColor());
            }
        }
    }

    IEnumerator CambiarColor()
    {
        float tiempo = 0f;

        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionTransicion;

            materialInstancia.SetColor("_BaseColor", Color.Lerp(colorInicialBase, colorVisto, t));
            materialInstancia.SetColor("_EmissionColor", Color.Lerp(colorInicialEmission, colorVisto, t));
            yield return null;
        }

        materialInstancia.SetColor("_BaseColor", colorVisto);
        materialInstancia.SetColor("_EmissionColor", colorVisto);
    }
}