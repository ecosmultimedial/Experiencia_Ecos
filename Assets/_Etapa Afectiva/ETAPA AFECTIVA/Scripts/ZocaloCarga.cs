using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(CanvasGroup))]
public class ZocaloCarga : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Configuración de fade")]
    [SerializeField] private float duracionFade = 0.4f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (videoPlayer != null)
            videoPlayer.isLooping = true;

        OcultarInmediato();
    }

    /// Muestra el zócalo con fade-in y arranca el video en loop.
    public void Mostrar()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        gameObject.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(1f));
    }

    /// Hace fade-out, frena el video y recién al terminar el fade desactiva el GameObject.
    /// Se usa con "yield return StartCoroutine(...)" para esperar a que termine antes de seguir el flujo.
    public IEnumerator OcultarYEsperar()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f));
        yield return fadeCoroutine;

        if (videoPlayer != null)
            videoPlayer.Stop();

        gameObject.SetActive(false);
    }

    /// Corte abrupto, sin fade — para usar cuando el jugador se va antes de tiempo (OnTriggerExit).
    public void OcultarInmediato()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (videoPlayer != null)
            videoPlayer.Stop();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(float alphaDestino)
    {
        float alphaInicial = canvasGroup.alpha;
        float tiempo = 0f;

        canvasGroup.interactable = alphaDestino > 0f;
        canvasGroup.blocksRaycasts = alphaDestino > 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracionFade);
            canvasGroup.alpha = Mathf.Lerp(alphaInicial, alphaDestino, t);
            yield return null;
        }

        canvasGroup.alpha = alphaDestino;
        fadeCoroutine = null;
    }
}