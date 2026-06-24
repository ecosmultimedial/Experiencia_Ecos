using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject pressXCanvas;

    [Header("Puerta")]
    public Animator doorAnimator;
    public AudioSource doorSound;

    private bool playerNear = false;
    private bool doorOpened = false;

    void Start()
    {
        pressXCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerNear && !doorOpened && Input.GetKeyDown(KeyCode.X))
        {
            AbrirPuerta();
        }
    }

    void AbrirPuerta()
    {
        doorOpened = true;
        pressXCanvas.SetActive(false);
        doorAnimator.SetTrigger("Abrir");

        if (doorSound != null)
            doorSound.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            pressXCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            pressXCanvas.SetActive(false);
        }
    }
}