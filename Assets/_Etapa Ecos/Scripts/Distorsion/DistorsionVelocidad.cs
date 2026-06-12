using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DistorsionVelocidad : MonoBehaviour
{
    [Header("Referencias")]
    public Volume volumenPostPro;
    public RecordatorioCanvas recordatorio;
    public CharacterController characterController;

    [Header("Configuración")]
    public float velocidadLimite = 1f;
    public float distorsionMaxima = -1f;
    public float velocidadTransicion = 3f;

    private LensDistortion lensDistortion;
    private bool recordatorioMostrado = false;

    void Start()
    {
        bool encontrado = volumenPostPro.profile.TryGet(out lensDistortion);
        Debug.Log("Lens Distortion encontrado: " + encontrado);
    }

    void Update()
    {
        if (lensDistortion == null) return;

        float velocidadActual = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z).magnitude;
        Debug.Log("Velocidad: " + velocidadActual + " | Distorsion: " + lensDistortion.intensity.value);

        float targetDistorsion = 0f;

        if (velocidadActual > velocidadLimite)
        {
            float t = Mathf.InverseLerp(velocidadLimite, velocidadLimite * 1.5f, velocidadActual);
            targetDistorsion = Mathf.Lerp(0f, distorsionMaxima, t);

            if (!recordatorioMostrado)
            {
                recordatorioMostrado = true;
                recordatorio.MostrarRecordatorio();
            }
        }

        lensDistortion.intensity.value = Mathf.Lerp(
            lensDistortion.intensity.value,
            targetDistorsion,
            Time.deltaTime * velocidadTransicion
        );
    }
}