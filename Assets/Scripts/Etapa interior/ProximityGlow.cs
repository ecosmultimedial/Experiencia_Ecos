using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityGlow : MonoBehaviour
{
    public Renderer cubeRenderer;

    public Color baseColor = Color.white;
    public Color glowColor = Color.white;

    public float baseIntensity = 1f;
    public float glowIntensity = 5f;

    public AudioSource audioSource;

    public bool specialCube = false;

    public CubeEventManager eventManager;

    private Material materialInstance;

    void Start()
    {
        materialInstance = cubeRenderer.material;
        SetEmission(baseColor, baseIntensity);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetEmission(glowColor, glowIntensity);

            if (audioSource != null)
            {
                audioSource.Play();
            }

            if (specialCube && eventManager != null)
            {
                eventManager.StartEvent();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetEmission(baseColor, baseIntensity);
        }
    }

    void SetEmission(Color color, float intensity)
    {
        materialInstance.SetColor("_EmissionColor", color * intensity);
    }
}