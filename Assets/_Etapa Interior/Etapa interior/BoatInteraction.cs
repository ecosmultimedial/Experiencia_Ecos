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
    public AudioSource boatAudioSource; // Arrastrá el AudioSource de la canoa acá

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

    void LateUpdate()
    {
        if (playerOnBoat)
        {
            player.transform.position = playerPoint.position;
        }
    }

    void SubirAlBote()
    {
        controller.enabled = false;
        player.transform.position = playerPoint.position;
        player.transform.rotation = playerPoint.rotation;
        playerController.MoveSpeed = 0f;
        playerController.SprintSpeed = 0f;
        playerOnBoat = true;
        pressEUI.SetActive(false);
        boatAnimator.SetBool("startBoat", true);

        // Reproducir sonido del agua
        if (boatAudioSource != null)
            boatAudioSource.Play();
    }

    public void DetenerSonidoCanoa()
    {
        if (boatAudioSource != null)
            boatAudioSource.Stop();
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