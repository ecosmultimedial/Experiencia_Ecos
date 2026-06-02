using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupContinuar : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image imagenCartel;
    public Button botonContinuar;

    [Tooltip("Panel raíz del popup. Si lo dejás vacío, usa este mismo GameObject.")]
    public GameObject panel;

    [Tooltip("CanvasGroup para hacer el fade. Si lo dejás vacío, lo busca en el panel.")]
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    [Tooltip("Duración del fade-in en segundos")]
    public float duracionFadeIn = 0.5f;

    [Tooltip("Duración del fade-out en segundos")]
    public float duracionFadeOut = 0.3f;

    private Action callbackContinuar;
    private bool estaAbierto = false;
    private bool enTransicion = false;

    void Awake()
    {
        if (panel == null) panel = gameObject;
        if (canvasGroup == null) canvasGroup = panel.GetComponent<CanvasGroup>();

        panel.SetActive(false);

        if (botonContinuar != null)
            botonContinuar.onClick.AddListener(Cerrar);
    }

    void Update()
    {
        if (!estaAbierto || enTransicion) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Cerrar();
        }
    }

    public void Mostrar(Sprite imagen, Action onContinuar)
    {
        if (imagenCartel != null && imagen != null)
            imagenCartel.sprite = imagen;

        callbackContinuar = onContinuar;
        panel.SetActive(true);
        estaAbierto = true;

        StartCoroutine(FadeIn());
    }

    public void Cerrar()
    {
        if (!estaAbierto || enTransicion) return;
        StartCoroutine(FadeOutYCerrar());
    }

    private IEnumerator FadeIn()
    {
        enTransicion = true;
        if (canvasGroup != null)
        {
            // Bloquear interacción mientras aparece
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float t = 0f;
            canvasGroup.alpha = 0f;
            while (t < duracionFadeIn)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, t / duracionFadeIn);
                yield return null;
            }
            canvasGroup.alpha = 1f;

            // Habilitar interacción cuando termina
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        enTransicion = false;
    }

    private IEnumerator FadeOutYCerrar()
    {
        enTransicion = true;
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float t = 0f;
            float alphaInicial = canvasGroup.alpha;
            while (t < duracionFadeOut)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.SmoothStep(alphaInicial, 0f, t / duracionFadeOut);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        panel.SetActive(false);
        estaAbierto = false;
        enTransicion = false;

        var cb = callbackContinuar;
        callbackContinuar = null;
        cb?.Invoke();
    }
}