using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzGuia : MonoBehaviour
{
    [Header("Configuración Fade")]
    [Tooltip("Duración del fade in y fade out en segundos")]
    public float duracionFade = 1f;

    [Header("Configuración Detección")]
    [Tooltip("Distancia a la que el jugador activa el fade out")]
    public float radioDeteccion = 3f;

    private Renderer rend;
    private Material mat;
    private Color colorEmisivoOriginal;
    private Transform jugador;

    private enum Estado { Inactiva, Activa, Completada }
    private Estado estado = Estado.Inactiva;

    void Awake()
    {
        // Busca el Renderer en este objeto o en cualquiera de sus hijos
        rend = GetComponentInChildren<Renderer>();
        // .material crea una instancia única para esta luz
        mat = rend.material;

        // Guardamos el color emisivo original (el que tiene en el editor)
        colorEmisivoOriginal = mat.GetColor("_EmissionColor");

        // Arrancamos apagadas
        mat.SetColor("_EmissionColor", Color.black);
        mat.EnableKeyword("_EMISSION");
    }

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) jugador = obj.transform;
    }

    void Update()
    {
        if (estado != Estado.Activa || jugador == null) return;

        float dist = Vector3.Distance(transform.position, jugador.position);
        if (dist <= radioDeteccion)
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            float k = t / duracionFade;
            mat.SetColor("_EmissionColor", Color.Lerp(Color.black, colorEmisivoOriginal, k));
            yield return null;
        }
        mat.SetColor("_EmissionColor", colorEmisivoOriginal);
        estado = Estado.Activa;
    }

    private IEnumerator FadeOut()
    {
        estado = Estado.Completada; // se marca ya, así no se redispara

        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            float k = t / duracionFade;
            mat.SetColor("_EmissionColor", Color.Lerp(colorEmisivoOriginal, Color.black, k));
            yield return null;
        }
        mat.SetColor("_EmissionColor", Color.black);
        gameObject.SetActive(false);
    }
}