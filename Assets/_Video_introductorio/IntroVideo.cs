using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button botonSaltarVideo;
    public string escenaSiguiente = "etapa desconexion";

    private float tiempoParaMostrarBoton = 27f; // En segundos
    private float videoDuration;
    private bool botonMostrado = false;

    void Start()
    {
        // Ocultar cursor durante el video
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtener duración del video
        videoDuration = (float)videoPlayer.clip.length;

        // Asegurarse que el botón esté oculto al inicio
        if (botonSaltarVideo != null)
        {
            botonSaltarVideo.gameObject.SetActive(false);
        }

        // Solo agregar el listener del botón
        if (botonSaltarVideo != null)
        {
            botonSaltarVideo.onClick.AddListener(SaltarVideo);
        }
    }

    void Update()
    {
        // Si el video está reproduciéndose, monitorear el tiempo
        if (videoPlayer.isPlaying)
        {
            float tiempoActual = (float)videoPlayer.time;

            // Mostrar botón cuando llega a los 27 segundos
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
            Cursor.lockState = CursorLockMode.Confined; // Permitir ver cursor
            Cursor.visible = true;
        }
    }

    void SaltarVideo()
    {
        // Cuando hace clic en el botón
        videoPlayer.Stop();
        SceneManager.LoadScene(escenaSiguiente);
    }
}