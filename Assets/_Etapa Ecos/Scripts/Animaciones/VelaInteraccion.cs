using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelaInteraccion : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject flame;
    public CanvasGroup promptCanvasGroup;
    public GameObject promptCanvas;
    public PopupContinuar popup;
    public Sprite imagenPopup;
    public MonoBehaviour scriptMovimientoPlayer;

    [Header("Configuración")]
    public float duracionFadePrompt = 0.5f;
    public float segundosAntesDelPopup = 3f;

    private bool playerDentro = false;
    private bool yaUsado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || yaUsado) return;
        playerDentro = true;
        promptCanvas.SetActive(true);
        StartCoroutine(FadePrompt(0f, 1f));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerDentro = false;
        StartCoroutine(FadeYOcultar());
    }

    private void Update()
    {
        if (playerDentro && !yaUsado && Input.GetKeyDown(KeyCode.E))
        {
            yaUsado = true;
            playerDentro = false;
            StartCoroutine(SecuenciaVela());
        }
    }

    private IEnumerator SecuenciaVela()
    {
        // Ocultar prompt
        yield return StartCoroutine(FadeYOcultar());

        // Activar llama
        flame.SetActive(true);

        // Esperar antes del popup
        yield return new WaitForSeconds(segundosAntesDelPopup);

        // Bloquear player y mostrar popup
        if (scriptMovimientoPlayer != null) scriptMovimientoPlayer.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (popup != null)
            popup.Mostrar(imagenPopup, OnPopupCerrado);
        else
            OnPopupCerrado();
    }

    private void OnPopupCerrado()
    {
        if (scriptMovimientoPlayer != null) scriptMovimientoPlayer.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator FadePrompt(float desde, float hasta)
    {
        float tiempo = 0f;
        while (tiempo < duracionFadePrompt)
        {
            tiempo += Time.deltaTime;
            promptCanvasGroup.alpha = Mathf.Lerp(desde, hasta, tiempo / duracionFadePrompt);
            yield return null;
        }
        promptCanvasGroup.alpha = hasta;
    }

    private IEnumerator FadeYOcultar()
    {
        yield return StartCoroutine(FadePrompt(promptCanvasGroup.alpha, 0f));
        promptCanvas.SetActive(false);
    }
}