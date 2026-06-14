using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartelRespuesta : MonoBehaviour
{
    [Header("Carteles (en orden)")]
    public CanvasGroup[] canvasGroups;
    public TextMeshProUGUI[] textos;

    [Header("Tiempos")]
    public float duracionFade = 1f;

    private int indiceActual = -1;

    void Start()
    {
        for (int i = 0; i < textos.Length; i++)
        {
            textos[i].text = PlayerPrefs.GetString("Respuesta_" + i, "...");
        }

        foreach (var cg in canvasGroups)
        {
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void MostrarSiguiente()
    {
        StartCoroutine(TransicionCarteles());
    }

    public void CerrarUltimo()
    {
        if (indiceActual >= 0 && indiceActual < canvasGroups.Length)
        {
            StartCoroutine(Fade(canvasGroups[indiceActual], 1f, 0f));
        }
    }

    private IEnumerator TransicionCarteles()
    {
        if (indiceActual >= 0 && indiceActual < canvasGroups.Length)
        {
            yield return StartCoroutine(Fade(canvasGroups[indiceActual], 1f, 0f));
        }

        indiceActual++;

        if (indiceActual < canvasGroups.Length)
        {
            yield return StartCoroutine(Fade(canvasGroups[indiceActual], 0f, 1f));
        }
    }

    private IEnumerator Fade(CanvasGroup cg, float desde, float hasta)
    {
        float t = 0f;
        cg.alpha = desde;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(desde, hasta, t / duracionFade);
            yield return null;
        }

        cg.alpha = hasta;
    }
}