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
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            SubirAlBote();
        }

        // Mantener al jugador en el punto del bote
        if (playerOnBoat)
        {
            controller.enabled = false;

            player.transform.position = playerPoint.position;
            player.transform.rotation = playerPoint.rotation;

            controller.enabled = true;
        }
    }

    void SubirAlBote()
    {
        controller.enabled = false;

        player.transform.position = playerPoint.position;
        player.transform.rotation = playerPoint.rotation;

        controller.enabled = true;

        // Bloquear movimiento
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