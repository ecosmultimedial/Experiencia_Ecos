using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject continueCanvas;

    [Header("Siluetas")]
    public List<Renderer> siluetas;

    [Header("Portal")]
    public GameObject portal;
    public AudioSource sonidoPortal; // Arrastrá acá el Audio Source con el sonido del portal

    [Header("Tiempos")]
    public float delayAntesDeBoton = 2f;
    public float fadeDuration = 3f;

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
        {
            ContinueExperience();
        }
    }

    public void ContinueExperience()
    {
        canvasVisible = false;
        continueCanvas.SetActive(false);
        StartCoroutine(DesvaneceYMuestraPortal());
    }

    IEnumerator DesvaneceYMuestraPortal()
    {
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

        if (portal != null)
        {
            portal.SetActive(true);

            // Reproducir sonido del portal al aparecer
            if (sonidoPortal != null)
                sonidoPortal.Play();
        }
    }
}