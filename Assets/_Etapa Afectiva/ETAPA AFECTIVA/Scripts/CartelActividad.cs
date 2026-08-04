using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;

public class CartelActividad : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button botonContinuar;

    [Header("Sistema de luces guía")]
    [SerializeField] private SistemaLucesGuia sistemaLuces;

    [Header("Bloqueo de cámara")]
    [SerializeField] private Transform puntoVista; // <- PuntoVista que creaste
    [SerializeField] private float velocidadTransicion = 3f; // Velocidad de transición

    private Image imagenBoton;
    private bool actividadCompletada = false;
    private FirstPersonController playerController;
    private CinemachineVirtualCamera virtualCamera;
    private Coroutine transicionCamara;

    private void Awake()
    {
        if (botonContinuar != null)
            imagenBoton = botonContinuar.GetComponent<Image>();
        playerController = FindObjectOfType<FirstPersonController>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (botonContinuar != null)
        {
            botonContinuar.onClick.AddListener(CompletarActividad);
            OcultarBoton();
        }
    }

    private void Update()
    {
        if (!actividadCompletada && botonContinuar != null && botonContinuar.interactable)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                CompletarActividad();
        }
    }

    public void BloquearMovimiento()
    {
        // Detener al player
        if (playerController != null)
            playerController.enabled = false;

        // Deshabilitar input de la cámara
        if (virtualCamera != null)
        {
            var inputProvider = virtualCamera.GetComponent<MonoBehaviour>();
            if (inputProvider != null)
                inputProvider.enabled = false;
        }

        // Transición suave de la cámara hacia puntoVista
        if (transicionCamara != null)
            StopCoroutine(transicionCamara);

        if (puntoVista != null)
            transicionCamara = StartCoroutine(TransicionCamara());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator TransicionCamara()
    {
        Transform camaraTransform = virtualCamera.transform;
        Vector3 posInicial = camaraTransform.position;
        Quaternion rotInicial = camaraTransform.rotation;

        Vector3 posFinal = puntoVista.position;
        Quaternion rotFinal = puntoVista.rotation;

        float tiempo = 0f;
        float duracion = 1f / velocidadTransicion; // Ajusta la duración según la velocidad

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);

            camaraTransform.position = Vector3.Lerp(posInicial, posFinal, t);
            camaraTransform.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);

            yield return null;
        }

        camaraTransform.position = posFinal;
        camaraTransform.rotation = rotFinal;
    }

    private void DesbloquearMovimiento()
    {
        if (playerController != null)
            playerController.enabled = true;

        if (virtualCamera != null)
        {
            var inputProvider = virtualCamera.GetComponent<MonoBehaviour>();
            if (inputProvider != null)
                inputProvider.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MostrarBoton()
    {
        if (actividadCompletada) return;
        botonContinuar.interactable = true;
        SetAlpha(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OcultarBoton()
    {
        if (actividadCompletada) return;
        botonContinuar.interactable = false;
        SetAlpha(0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetAlpha(float a)
    {
        if (imagenBoton == null) return;
        Color c = imagenBoton.color;
        c.a = a;
        imagenBoton.color = c;
    }

    private void CompletarActividad()
    {
        if (actividadCompletada) return;
        actividadCompletada = true;
        botonContinuar.interactable = false;
        SetAlpha(0f);
        DesbloquearMovimiento();

        if (sistemaLuces != null)
            sistemaLuces.IniciarGrupo3();

        Debug.Log("Actividad del cartel completada. Tercer grupo de luces activado.");
    }

    public bool EstaCompletada => actividadCompletada;
}