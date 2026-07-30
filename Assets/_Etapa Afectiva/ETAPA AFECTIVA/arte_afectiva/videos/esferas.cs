using System.Collections;
using UnityEngine;

public class esferas : MonoBehaviour
{
    [Header("Referencias")]
    public SpriteRenderer spriteVideo;
    public Renderer esferaRenderer;

    [Header("Fade")]
    public float duracionFade = 1f;

    [Header("Color al ser vista")]
    public Color colorVisto = new Color(0.41f, 0.91f, 0.92f);
    [Range(0f, 1f)]
    public float alphaVisto = 0.5f;

    private bool fueVista = false;
    private Color colorInicial;
    private Material materialInstancia;
    private Coroutine fadeActual;

    void Start()
    {
        materialInstancia = esferaRenderer.material;
        colorInicial = materialInstancia.GetColor("_BaseColor");

        materialInstancia.SetFloat("_Surface", 1f);
        materialInstancia.SetOverrideTag("RenderType", "Transparent");
        materialInstancia.renderQueue = 3000;

        if (spriteVideo != null)
        {
            Color c = spriteVideo.color;
            c.a = 0f;
            spriteVideo.color = c;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (fadeActual != null) StopCoroutine(fadeActual);
        fadeActual = StartCoroutine(FadeSprite(0f, 1f));
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!fueVista)
        {
            fueVista = true;
            StartCoroutine(CambiarColor());
            EsferasManager.Instance.EsferaVista();
        }
    }

    IEnumerator FadeSprite(float desdeAlpha, float hastaAlpha)
    {
        float tiempo = 0f;
        Color c = spriteVideo.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionFade;
            c.a = Mathf.Lerp(desdeAlpha, hastaAlpha, t);
            spriteVideo.color = c;
            yield return null;
        }

        c.a = hastaAlpha;
        spriteVideo.color = c;
    }

    IEnumerator CambiarColor()
    {
        Color colorFinal = new Color(colorVisto.r, colorVisto.g, colorVisto.b, alphaVisto);
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionFade;
            Color actual = Color.Lerp(colorInicial, colorFinal, t);
            materialInstancia.SetColor("_BaseColor", actual);
            yield return null;
        }

        materialInstancia.SetColor("_BaseColor", colorFinal);
    }
}