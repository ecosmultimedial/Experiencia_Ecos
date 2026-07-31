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
    
    public float detectionRadius = 3f;
    private bool playerCanExit = false;
    private bool hasExited = false;

    void Start()
    {
        pressQUI.SetActive(false);
    }

    void Update()
    {
        if (hasExited) return;
        if (!boatInteraction.playerOnBoat) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < detectionRadius && !playerCanExit)
        {
            playerCanExit = true;
            pressQUI.SetActive(true);
        }

        if (playerCanExit && Input.GetKeyDown(KeyCode.X))
        {
            BajarDelBote();
        }
    }

    void BajarDelBote()
    {
        hasExited = true;
        pressQUI.SetActive(false);
        boatInteraction.DetenerSonidoCanoa();
        player.transform.position = exitPoint.position;
        player.transform.rotation = exitPoint.rotation;
        controller.enabled = true;
        playerController.MoveSpeed = 4f;
        playerController.SprintSpeed = 6f;
        boatInteraction.playerOnBoat = false;
        boatInteraction.enabled = false;
    }
}