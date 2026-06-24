using UnityEngine;

public class Sonidoxilofon : MonoBehaviour
{
    public AudioClip sonido;
    public float volumen = 1f;

    private bool yasono = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yasono)
        {
            yasono = true;
            audioSource.clip = sonido;
            audioSource.volume = volumen;
            audioSource.Play();
        }
    }
}