using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionEffect : MonoBehaviour
{
    [Header("Shader Graph Material")]
    public Renderer targetRenderer;
    public float effectDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip collisionSound;

    // References de tu Shader Graph
    private readonly string noiseSpeedParam = "_NoiseSpeed";
    private readonly string noiseScaleParam = "_NoiseScale";
    private readonly string noiseContrastParam = "_NoiseContrast";

    private Material mat;
    private bool isEffectActive = false;

    void Start()
    {
        if (targetRenderer != null)
            mat = targetRenderer.material;

        SetEffectValues(0f, 0f, 0f);
    }

    // ✅ OnCollisionEnter: la pared NO es trigger, bloquea físicamente
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InvisibleWall") && !isEffectActive)
        {
            if (collisionSound != null)
                audioSource.PlayOneShot(collisionSound);

            StartCoroutine(PlayEffect());
        }
    }

    System.Collections.IEnumerator PlayEffect()
    {
        isEffectActive = true;
        float elapsed = 0f;

        // Fade IN
        while (elapsed < effectDuration * 0.3f)
        {
            float t = elapsed / (effectDuration * 0.3f);
            SetEffectValues(
                speed: Mathf.Lerp(0f, 1f, t),
                scale: Mathf.Lerp(0f, 1f, t),
                contrast: Mathf.Lerp(0f, 0.8f, t)
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fade OUT
        elapsed = 0f;
        while (elapsed < effectDuration * 0.7f)
        {
            float t = elapsed / (effectDuration * 0.7f);
            SetEffectValues(
                speed: Mathf.Lerp(1f, 0f, t),
                scale: Mathf.Lerp(1f, 0f, t),
                contrast: Mathf.Lerp(0.8f, 0f, t)
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetEffectValues(0f, 0f, 0f);
        isEffectActive = false;
    }

    void SetEffectValues(float speed, float scale, float contrast)
    {
        if (mat == null) return;
        mat.SetFloat(noiseSpeedParam, speed);
        mat.SetFloat(noiseScaleParam, scale);
        mat.SetFloat(noiseContrastParam, contrast);
    }

    void OnDestroy()
    {
        if (mat != null) Destroy(mat);
    }
}




