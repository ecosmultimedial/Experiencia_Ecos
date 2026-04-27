using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class BoatExitSystem : MonoBehaviour
{
    public GameObject player;

    public Transform exitPoint;

    public GameObject pressQUI;

    public FirstPersonController playerController;

    public CharacterController controller;

    public BoatInteraction boatInteraction;

    private bool playerCanExit = false;

    void Start()
    {
        pressQUI.SetActive(false);
    }

    void Update()
    {
        if (playerCanExit && Input.GetKeyDown(KeyCode.Q))
        {
            BajarDelBote();
        }
    }

    void BajarDelBote()
    {
        controller.enabled = false;

        player.transform.position = exitPoint.position;
        player.transform.rotation = exitPoint.rotation;

        controller.enabled = true;

        // Restaurar movimiento
        playerController.MoveSpeed = 4f;
        playerController.SprintSpeed = 6f;

        // Desactivar seguimiento del bote
        boatInteraction.enabled = false;

        pressQUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanExit = true;
            pressQUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanExit = false;
            pressQUI.SetActive(false);
        }
    }
}