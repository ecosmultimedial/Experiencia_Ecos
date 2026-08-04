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
    [SerializeField] private Transform puntoVista;
    [SerializeField] private float velocidadTransicion = 3f;

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
        Debug.Log("=== BloquearMovimiento() LLAMADO ===");

        if (playerController != null)
            playerController.enabled = false;

        if (virtualCamera != null)
        {
            virtualCamera.enabled = false;
            Debug.Log("✓ Cinemachine deshabilitado");
        }

        if (transicionCamara != null)
            StopCoroutine(transicionCamara);

        if (puntoVista != null)
        {
            Debug.Log("✓ Iniciando transición de cámara...");
            transicionCamara = StartCoroutine(TransicionCamara());
        }
        else
        {
            Debug.LogError("✗ puntoVista es NULL");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator TransicionCamara()
    {
        // Obtener la MAIN CAMERA, no la virtualCamera
        Camera camaraMain = Camera.main;
        if (camaraMain == null)
        {
            Debug.LogError("✗ Main Camera no encontrada");
            yield break;
        }

        Transform camaraTransform = camaraMain.transform;
        Vector3 posInicial = camaraTransform.position;
        Quaternion rotInicial = camaraTransform.rotation;

        Vector3 posFinal = puntoVista.position;
        Quaternion rotFinal = puntoVista.rotation;

        Debug.Log($"Posición inicial: {posInicial}");
        Debug.Log($"Posición final: {posFinal}");

        float tiempo = 0f;
        float duracion = 1f / velocidadTransicion;

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

        Debug.Log("✓ Transición completada");
    }

    private void DesbloquearMovimiento()
    {
        if (playerController != null)
            playerController.enabled = true;

        // CRÍTICO: Reactivar Cinemachine
        if (virtualCamera != null)
        {
            virtualCamera.enabled = true;
            Debug.Log("✓ Cinemachine reactivado");
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