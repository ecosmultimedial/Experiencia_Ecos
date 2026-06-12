using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscaleraRotatoria : MonoBehaviour
{
    [Header("Configuración de rotación")]
    [Tooltip("Tiempo en segundos que tarda la escalera en rotar")]
    public float duracionRotacion = 2f;
    [Tooltip("Ángulo de ida (A→B). Define cuánto rota desde la posición inicial.")]
    public float anguloIda = 90f;
    [Tooltip("Ángulo de vuelta (B→A). Normalmente es el opuesto al de ida (ej: si ida es 90, vuelta es -90), pero podés usar otro valor si querés que rote por un camino distinto.")]
    public float anguloVuelta = -90f;

    private Quaternion rotacionA;
    private bool estaRotando = false;
    private bool enPosicionB = false;

    void Start()
    {
        rotacionA = transform.rotation;
    }

    public void RotarHaciaB()
    {
        if (estaRotando || enPosicionB) return;
        Quaternion destino = transform.rotation * Quaternion.Euler(0f, anguloIda, 0f);
        StartCoroutine(RotarRutina(destino, true));
    }

    public void RotarHaciaA()
    {
        if (estaRotando || !enPosicionB) return;
        Quaternion destino = transform.rotation * Quaternion.Euler(0f, anguloVuelta, 0f);
        StartCoroutine(RotarRutina(destino, false));
    }

    public void Toggle()
    {
        if (estaRotando) return;
        if (enPosicionB) RotarHaciaA();
        else RotarHaciaB();
    }

    private IEnumerator RotarRutina(Quaternion rotacionFinal, bool destinoEsB)
    {
        estaRotando = true;
        Quaternion rotacionInicial = transform.rotation;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionRotacion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / duracionRotacion);
            float tSuave = Mathf.SmoothStep(0f, 1f, t);
            transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, tSuave);
            yield return null;
        }

        transform.rotation = rotacionFinal;
        enPosicionB = destinoEsB;
        estaRotando = false;
    }
}