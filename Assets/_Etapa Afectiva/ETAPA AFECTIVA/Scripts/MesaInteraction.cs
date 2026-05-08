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

    void Update()
    {
        if (jugadorCerca && !hojaAbierta && Input.GetKeyDown(KeyCode.E))
            AbrirHoja();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
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

        // Deshabilitar input de cámara Cinemachine
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

        // Rehabilitar input de cámara Cinemachine
        var inputProvider = virtualCamera.GetComponent<MonoBehaviour>();
        if (inputProvider != null)
            inputProvider.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (jugadorCerca)
            promptHUD.SetActive(true);
    }
}