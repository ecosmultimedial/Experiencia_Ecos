using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float duracionFade = 1f;
    public float DuracionFade => duracionFade;

    private bool vieneDeFadeOut = false;  // marca si la escena anterior hizo fade negro

    void Awake()
    {
        // Singleton: si ya existe uno, este se autodestruye
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe un FadeManager, este nuevo se mata
            // pero antes le pasa el estado al que sobrevive por si hace falta
            Destroy(gameObject);
            return;
        }

        // Arranca transparente
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    private void AlCargarEscena(Scene scene, LoadSceneMode mode)
    {
        // Solo hace fade in si la escena anterior hizo fade out negro
        if (vieneDeFadeOut)
        {
            vieneDeFadeOut = false;
            StartCoroutine(FadeIn());
        }
        else
        {
            // Asegurarse de que esté transparente si no corresponde fade in
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(FadeOutYCargar(nombreEscena));
    }

    private IEnumerator FadeOutYCargar(string nombreEscena)
    {
        yield return StartCoroutine(FadeOut());
        vieneDeFadeOut = true;
        SceneManager.LoadScene(nombreEscena);
    }

    private IEnumerator FadeOut()
    {
        fadeCanvasGroup.blocksRaycasts = true;
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(t / duracionFade);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1f;
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(t / duracionFade);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}