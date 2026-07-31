using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeEventManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject continueCanvas;

    [Header("Siluetas")]
    public List<Renderer> siluetas;

    [Header("Portal")]
    public GameObject portal;
    public AudioSource sonidoPortal;
    public float delayAntesDeSonido = 3f;


    [Header("Fade In Portal")]
    public Image panelNegro;
    public float duracionFadePortal = 2.5f;

    [Header("Tiempos")]
    public float delayAntesDeBoton = 2f;
    public float fadeDuration = 3f;

    [Header("Referencia al trigger de la silueta")]
    public ProximityGlow proximityGlow;

    private bool activated = false;
    private bool canvasVisible = false;

    public void StartEvent()
    {
        if (!activated)
        {
            activated = true;
            StartCoroutine(MostrarBotonContinuar());
        }
    }

    IEnumerator MostrarBotonContinuar()
    {
        yield return new WaitForSeconds(delayAntesDeBoton);
        continueCanvas.SetActive(true);
        canvasVisible = true;
    }

    void Update()
    {
        if (canvasVisible && Input.GetKeyDown(KeyCode.Return))
            ContinueExperience();
    }

    public void ContinueExperience()
    {
        canvasVisible = false;
        continueCanvas.SetActive(false);

        // Fade out suave de la voz antes de continuar
        if (proximityGlow != null)
            proximityGlow.FadeOutVoz();

        StartCoroutine(DesvaneceYMuestraPortal());
    }

    IEnumerator DesvaneceYMuestraPortal()
    {
        // Desvanecer siluetas
        float time = 0;
        List<Material> materials = new List<Material>();
        foreach (Renderer r in siluetas)
            materials.Add(r.material);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            foreach (Material m in materials)
            {
                Color color = m.color;
                color.a = alpha;
                m.color = color;
            }
            yield return null;
        }

        foreach (Renderer r in siluetas)
            r.gameObject.SetActive(false);

        // FADE IN a negro (pantalla se oscurece gradualmente)
        if (panelNegro != null)
        {
            float t = 0f;
            while (t < duracionFadePortal)
            {
                t += Time.deltaTime;
                float alpha = Mathf.SmoothStep(0f, 1f, t / duracionFadePortal);
                Color c = panelNegro.color;
                c.a = alpha;
                panelNegro.color = c;
                yield return null;
            }
            Color final = panelNegro.color;
            final.a = 1f;
            panelNegro.color = final;
        }

        // Esperar dos frames y activar portal mientras la pantalla está negra
        yield return null;
        yield return null;

        if (portal != null)
            portal.SetActive(true);

        yield return null;

        // Reproducir sonido con delay
        if (sonidoPortal != null)
        {
            yield return new WaitForSeconds(delayAntesDeSonido);
            sonidoPortal.Play();
        }

        // FADE OUT desde negro (pantalla se aclara revelando el portal)
        if (panelNegro != null)
        {
            float t = 0f;
            while (t < duracionFadePortal)
            {
                t += Time.deltaTime;
                float alpha = Mathf.SmoothStep(1f, 0f, t / duracionFadePortal);
                Color c = panelNegro.color;
                c.a = alpha;
                panelNegro.color = c;
                yield return null;
            }
            Color final = panelNegro.color;
            final.a = 0f;
            panelNegro.color = final;
        }
    }
}