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
    [SerializeField] private Transform puntoFijo; // <- arrastrás PuntoFijo acá

    private Image imagenBoton;
    private bool actividadCompletada = false;

    private FirstPersonController playerController;
    private CinemachineVirtualCamera virtualCamera;

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
        if (playerController != null)
            playerController.enabled = false;

        if (virtualCamera != null)
        {
            // Apuntar la cámara hacia el punto fijo
            if (puntoFijo != null)
            {
                Vector3 direccion = puntoFijo.position - virtualCamera.transform.position;
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                virtualCamera.transform.rotation = rotacionObjetivo;
            }

            // Deshabilitar el input de la cámara para que no se pueda mover
            var inputProvider = virtualCamera.GetComponent<MonoBehaviour>();
            if (inputProvider != null)
                inputProvider.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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