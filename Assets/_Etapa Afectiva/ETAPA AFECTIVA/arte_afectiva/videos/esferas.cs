using UnityEngine;
using UnityEngine.Video;

public class TriggerVideo : MonoBehaviour
{
    public GameObject videoUI;
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoUI.SetActive(false); // oculto al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            videoUI.SetActive(true);
            videoPlayer.Play();
        }
    }
    
     void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            videoPlayer.Stop();
            videoUI.SetActive(false);
        }
    }
}