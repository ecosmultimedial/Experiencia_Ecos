using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button botonSaltarVideo;
    public CanvasGroup fadePanel;
    public CanvasGroup botonCanvasGroup;
    public string escenaSiguiente = "etapa desconexion";

    public float duracionFadeOut = 2f;

    private float tiempoParaMostrarBoton = 36f;
    private float videoDuration;
    private bool botonMostrado = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        videoDuration = (float)videoPlayer.clip.length;

        if (botonSaltarVideo != null)
        {
            botonSaltarVideo.gameObject.SetActive(false);
            botonSaltarVideo.onClick.AddListener(SaltarVideo);
        }

        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
        }

        if (botonCanvasGroup != null)
        {
            botonCanvasGroup.alpha = 1f;
        }

        // Listener para cuando el video termina
        videoPlayer.loopPointReached += TerminoVideo;
    }

    void Update()
    {
        if (videoPlayer.isPlaying)
        {
            float tiempoActual = (float)videoPlayer.time;

            if (tiempoActual >= tiempoParaMostrarBoton && !botonMostrado)
            {
                MostrarBoton();
                botonMostrado = true;
            }
        }
    }

    void MostrarBoton()
    {
        if (botonSaltarVideo != null)
        {
            botonSaltarVideo.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    void SaltarVideo()
    {
        StartCoroutine(FadeOutYCargarEscena());
    }

    void TerminoVideo(VideoPlayer vp)
    {
        // Cuando el video termina naturalmente
        SceneManager.LoadScene(escenaSiguiente);
    }

    System.Collections.IEnumerator FadeOutYCargarEscena()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionFadeOut)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, tiempoTranscurrido / duracionFadeOut);

            if (fadePanel != null)
                fadePanel.alpha = alpha;

            if (botonCanvasGroup != null)
                botonCanvasGroup.alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / duracionFadeOut);

            yield return null;
        }

        fadePanel.alpha = 1f;
        if (botonCanvasGroup != null)
            botonCanvasGroup.alpha = 0f;

        videoPlayer.Stop();
        SceneManager.LoadScene(escenaSiguiente);
    }
}