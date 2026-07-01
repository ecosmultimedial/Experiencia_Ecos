using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string escenaSiguiente = "etapa desconexion";

    void Start()
    {
        // Ocultar cursor durante el video
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        videoPlayer.loopPointReached += TerminoVideo;
    }

    void TerminoVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(escenaSiguiente);
    }
}