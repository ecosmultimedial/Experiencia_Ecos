using UnityEngine;

public class MusicZoneTrigger : MonoBehaviour
{
    public AudioSource musicaFondo; // Arrastrá el Audio Source de AUDIO GENERAL

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (musicaFondo != null)
                musicaFondo.Pause();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (musicaFondo != null)
                musicaFondo.UnPause();
        }
    }
}
