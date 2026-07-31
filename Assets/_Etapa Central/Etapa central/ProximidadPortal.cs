using System.Collections;
using UnityEngine;

public class ProximidadPortal : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Distancias")]
    [Tooltip("A partir de esta distancia, el sonido empieza a escucharse (muy tenue).")]
    public float distanciaMaxima = 10f;
    [Tooltip("A esta distancia o menos, el sonido esta en su volumen maximo.")]
    public float distanciaMinima = 2f;

    [Header("Volumen")]
    public float volumenMaximo = 1f;

    private Transform player;
    private bool fadeForzadoActivo = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 0f;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (fadeForzadoActivo || player == null || audioSource == null) return;

        float distancia = Vector3.Distance(player.position, transform.position);
        float t = Mathf.InverseLerp(distanciaMaxima, distanciaMinima, distancia);
        audioSource.volume = Mathf.Lerp(0f, volumenMaximo, t);
    }

    public void ForzarFadeOut(float duracion)
    {
        if (fadeForzadoActivo) return;
        fadeForzadoActivo = true;
        StartCoroutine(FadeOutCoroutine(duracion));
    }

    private IEnumerator FadeOutCoroutine(float duracion)
    {
        float volumenInicial = audioSource.volume;
        float t = 0f;

        while (t < duracion)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}