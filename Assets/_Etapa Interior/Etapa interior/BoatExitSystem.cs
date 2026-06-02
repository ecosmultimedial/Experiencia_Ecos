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

    public float detectionRadius = 8f;
    private bool playerCanExit = false;
    private bool hasExited = false;

    void Start()
    {
        pressQUI.SetActive(false);
    }

    void Update()
    {
        if (hasExited) return;

        // Solo chequear si el player está en la canoa
        if (!boatInteraction.playerOnBoat) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < detectionRadius && !playerCanExit)
        {
            playerCanExit = true;
            pressQUI.SetActive(true);
        }

        if (playerCanExit && Input.GetKeyDown(KeyCode.Q))
        {
            BajarDelBote();
        }
    }

    void BajarDelBote()
    {
        hasExited = true;
        pressQUI.SetActive(false);

        // Mover al exit point
        player.transform.position = exitPoint.position;
        player.transform.rotation = exitPoint.rotation;

        // Reactivar CharacterController
        controller.enabled = true;

        // Restaurar movimiento
        playerController.MoveSpeed = 4f;
        playerController.SprintSpeed = 6f;

        // Desactivar seguimiento
        boatInteraction.playerOnBoat = false;
        boatInteraction.enabled = false;
    }
}