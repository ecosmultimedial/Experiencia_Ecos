using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsferasManager : MonoBehaviour
{
    public static EsferasManager Instance;

    [Header("Referencias")]
    public GameObject portal;
    public int totalEsferas = 4;
    public float demora = 2f;

    [Header("Fade In del Portal")]
    public float duracionFadeIn = 1f;

    private int esferasVistas = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        portal.SetActive(false);
    }

    public void EsferaVista()
    {
        esferasVistas++;
        Debug.Log($"Esferas vistas: {esferasVistas}/{totalEsferas}");

        if (esferasVistas >= totalEsferas)
            StartCoroutine(ActivarPortalConDemora());
    }

    IEnumerator ActivarPortalConDemora()
    {
        yield return new WaitForSeconds(demora);

        portal.SetActive(true);
        Debug.Log("¡Portal activado!");

        yield return StartCoroutine(FadeInPortal());
    }

    IEnumerator FadeInPortal()
    {
        // Buscamos todos los renderers del portal (incluyendo hijos)
        Renderer[] renderers = portal.GetComponentsInChildren<Renderer>();

        // Guardamos los materiales y sus colores originales
        List<Material> materiales = new List<Material>();
        List<Color> coloresOriginales = new List<Color>();

        foreach (Renderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                materiales.Add(mat);
                coloresOriginales.Add(mat.color);
            }
        }

        float tiempo = 0f;

        while (tiempo < duracionFadeIn)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / duracionFadeIn);

            for (int i = 0; i < materiales.Count; i++)
            {
                Color c = coloresOriginales[i];
                materiales[i].color = new Color(c.r, c.g, c.b, c.a * alpha);
            }

            yield return null;
        }

        // Aseguramos que queden con su alpha original al final
        for (int i = 0; i < materiales.Count; i++)
        {
            materiales[i].color = coloresOriginales[i];
        }
    }
}