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
    public GameObject paredInvisible;
    [Header("Audio")]
    public AudioSource sonidoPortal;
    public AudioSource vozEnOff;
    public float duracionRealSonidoPortal = 10f; // Duración REAL sin silencio
    public float delayAntesDeLaVoz = 0f; // Ajustable para sincronizar
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
        if (proximityGlow != null)
            proximityGlow.FadeOutVoz();
        StartCoroutine(DesvaneceYMuestraPortal());
    }

    IEnumerator DesvaneceYMuestraPortal()
    {
        // Reproducir sonido del portal INMEDIATAMENTE
        if (sonidoPortal != null)
        {
            sonidoPortal.Play();
        }

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

        // FADE IN a negro
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

        yield return null;
        yield return null;
        if (portal != null)
            portal.SetActive(true);
        if (paredInvisible != null)
            paredInvisible.SetActive(true);

        yield return null;

        // FADE OUT desde negro
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

        // Esperar la duración REAL del sonido portal + delay ajustable
        yield return new WaitForSeconds(duracionRealSonidoPortal + delayAntesDeLaVoz);

        // Reproducir voz en off
        if (vozEnOff != null)
        {
            vozEnOff.Play();
            yield return new WaitForSeconds(vozEnOff.clip.length);
        }

        // Desactivar pared invisible
        if (paredInvisible != null)
            paredInvisible.SetActive(false);
    }
}