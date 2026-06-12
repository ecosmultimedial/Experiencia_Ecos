using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordatorioCanvas : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI texto;

    [Header("Configuración")]
    public float velocidadEscritura = 0.05f;
    public float tiempoEsperaAlFinal = 3f;
    public float duracionFade = 0.8f;

    private string mensajeCompleto = "Recordatorio: es importante que te muevas lento. Cada transformación lleva su tiempo.";

    public void MostrarRecordatorio()
    {
        gameObject.SetActive(true);
        StartCoroutine(RutinaRecordatorio());
    }

    private IEnumerator RutinaRecordatorio()
    {
        // Fade in
        texto.text = "";
        yield return StartCoroutine(FadeCanvas(0f, 1f));

        // Typewriter
        foreach (char letra in mensajeCompleto)
        {
            texto.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        // Espera
        yield return new WaitForSeconds(tiempoEsperaAlFinal);

        // Fade out
        yield return StartCoroutine(FadeCanvas(1f, 0f));

        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float desde, float hasta)
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