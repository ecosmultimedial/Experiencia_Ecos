using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortalFadeIn : MonoBehaviour
{
    [Header("Configuración")]
    public Image panelNegro;
    public float duracionFade = 2.5f;

    void OnEnable()
    {
        StartCoroutine(HacerFadeIn());
    }

    IEnumerator HacerFadeIn()
    {
        // Empezar completamente negro
        SetAlpha(1f);

        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(1f, 0f, tiempo / duracionFade);
            SetAlpha(t);
            yield return null;
        }

        SetAlpha(0f);
        panelNegro.gameObject.SetActive(false);
    }

    void SetAlpha(float alpha)
    {
        Color c = panelNegro.color;
        c.a = alpha;
        panelNegro.color = c;
    }
}