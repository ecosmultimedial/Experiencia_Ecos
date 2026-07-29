using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class MesaInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject hojaPanel;
    public GameObject promptHUD;
    public FirstPersonController playerController;
    public CinemachineVirtualCamera virtualCamera;

    private bool jugadorCerca = false;
    private bool hojaAbierta = false;
    [HideInInspector] public bool yaCompletado = false; // <- nuevo

    void Update()
    {
        if (jugadorCerca && !hojaAbierta && !yaCompletado && Input.GetKeyDown(KeyCode.E))
            AbrirHoja();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (!yaCompletado) // <- no mostrar prompt si ya terminó
                promptHUD.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            promptHUD.SetActive(false);
        }
    }

    public void AbrirHoja()
    {
        hojaAbierta = true;
        hojaPanel.SetActive(true);
        promptHUD.SetActive(false);
        playerController.enabled = false;

        var inputProvider = virtualCamera.GetComponent<MonoBehaviour>();
        if (inputProvider != null)
            inputProvider.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CerrarHoja()
    {
        hojaAbierta = false;
        hojaPanel.SetActive(false);
        playerController.enabled = true;

        var inputProvider = virtualCamera.GetComponent<MonoBehaviour>();
        if (inputProvider != null)
            inputProvider.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (jugadorCerca && !yaCompletado) // <- solo si no completó
            promptHUD.SetActive(true);
    }
}