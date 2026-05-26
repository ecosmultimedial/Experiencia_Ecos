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

    private bool playerNear = false;
    private bool playerOnBoat = false;

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

    void SubirAlBote()
    {
        // 1. Desactivar el CharacterController
        controller.enabled = false;

        // 2. Parentear el player a la canoa (se mueven juntos sin conflictos de física)
        player.transform.SetParent(playerPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        // 3. Bloquear movimiento pero dejar la cámara libre
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