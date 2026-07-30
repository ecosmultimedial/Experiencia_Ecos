using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoProximidad : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip audioClip;
    public bool loop = false;
    public bool detenerAlSalir = true;
    public bool reproducirSoloUnaVez = false;
    public float volumen = 1f;

    private AudioSource audioSource;
    private bool reproduciendo = false;
    private bool yaSeReproduco = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.volume = volumen;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Cuando el audio termina, reseteamos reproduciendo
        if (reproduciendo && !audioSource.isPlaying)
            reproduciendo = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (reproducirSoloUnaVez && yaSeReproduco)
                return;

            if (!reproduciendo)
            {
                audioSource.Play();
                reproduciendo = true;
                yaSeReproduco = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (detenerAlSalir)
            {
                audioSource.Stop();
                reproduciendo = false;
            }
        }
    }
}