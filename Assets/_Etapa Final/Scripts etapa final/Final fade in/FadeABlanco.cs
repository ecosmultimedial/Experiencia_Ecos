using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeABlanco : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroupFondo;
    public CanvasGroup canvasGroupQR;
    public GameObject canvasQR;

    [Header("Tiempos")]
    public float duracionFadeBlanco = 2f;
    public float esperaAntesDeQR = 2f;
    public float duracionFadeQR = 1.5f;

    void Start()
    {
        canvasGroupFondo.alpha = 0f;
        canvasGroupFondo.interactable = false;
        canvasGroupFondo.blocksRaycasts = false;

        canvasQR.SetActive(false);
        canvasGroupQR.alpha = 0f;
        canvasGroupQR.interactable = false;
        canvasGroupQR.blocksRaycasts = false;
    }

    public void IniciarSecuenciaFinal()
    {
        StartCoroutine(SecuenciaFinal());
    }

    private IEnumerator SecuenciaFinal()
    {
        // 1. Fade in a blanco
        float t = 0f;
        canvasGroupFondo.blocksRaycasts = true;
        while (t < duracionFadeBlanco)
        {
            t += Time.deltaTime;
            canvasGroupFondo.alpha = Mathf.Clamp01(t / duracionFadeBlanco);
            yield return null;
        }
        canvasGroupFondo.alpha = 1f;

        // 2. Esperar antes de mostrar el QR
        yield return new WaitForSeconds(esperaAntesDeQR);

        // 3. Fade in del canvas QR
        canvasQR.SetActive(true);
        t = 0f;
        while (t < duracionFadeQR)
        {
            t += Time.deltaTime;
            canvasGroupQR.alpha = Mathf.Clamp01(t / duracionFadeQR);
            yield return null;
        }
        canvasGroupQR.alpha = 1f;
        canvasGroupQR.interactable = true;
        canvasGroupQR.blocksRaycasts = true;

        // 4. Desbloquear cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}