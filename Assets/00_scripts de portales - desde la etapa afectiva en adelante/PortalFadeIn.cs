using System.Collections;
using UnityEngine;

public class PortalFadeIn : MonoBehaviour
{
    [Header("Configuración")]
    public float duracionFade = 2.5f;

    void OnEnable()
    {
        StartCoroutine(HacerFadeIn());
    }

    IEnumerator HacerFadeIn()
    {
        // Empezar en escala 0
        transform.localScale = Vector3.zero;

        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracionFade);

            // Curva suave — empieza lento y termina lento
            float tSuave = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.one * tSuave;

            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}