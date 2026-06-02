using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class BoatInteraction : MonoBehaviour
{
    public GameObject player;
    public Transform playerPoint;
    public GameObject pressEUI;
    public FirstPersonController playerController;
    public CharacterController controller;
    public Animator boatAnimator;

    [HideInInspector] public bool playerOnBoat = false;
    private bool playerNear = false;

    void Start()
    {
        pressEUI.SetActive(false);
    }

    void Update()
    {
        if (playerNear && !playerOnBoat && Input.GetKeyDown(KeyCode.E))
        {
            SubirAlBote();
        }
    }

    // LateUpdate para que siga la posición DESPUÉS de que la animación mueva la canoa
    void LateUpdate()
    {
        if (playerOnBoat)
        {
            // Seguir la canoa sin parentear — player queda en raíz de jerarquía
            player.transform.position = playerPoint.position;
        }
    }

    void SubirAlBote()
    {
        // Desactivar CharacterController y dejarlo desactivado
        controller.enabled = false;

        // Posicionar el player en el punto de la canoa
        player.transform.position = playerPoint.position;
        player.transform.rotation = playerPoint.rotation;

        // Bloquear movimiento, cámara libre
        playerController.MoveSpeed = 0f;
        playerController.SprintSpeed = 0f;

        playerOnBoat = true;
        pressEUI.SetActive(false);
        boatAnimator.SetBool("startBoat", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            pressEUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            pressEUI.SetActive(false);
        }
    }
}