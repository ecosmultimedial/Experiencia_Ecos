using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordatorioInicio : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;

    [Header("Configuración")]
    public float tiempoVisible = 10f;
    public float duracionFade = 1f;

    private void Start()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);
        StartCoroutine(RutinaRecordatorio());
    }

    private IEnumerator RutinaRecordatorio()
    {
        // Fade in
        yield return StartCoroutine(Fade(0f, 1f));

        // Esperar
        yield return new WaitForSeconds(tiempoVisible);

        // Fade out
        yield return StartCoroutine(Fade(1f, 0f));

        gameObject.SetActive(false);
    }

    private IEnumerator Fade(float desde, float hasta)
    {
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(desde, hasta, tiempo / duracionFade);
            yield return null;
        }
        canvasGroup.alpha = hasta;
    }
}