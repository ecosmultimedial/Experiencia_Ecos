using UnityEngine;
using StarterAssets;
using Cinemachine;

public class SillaInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform puntoSentado;
    public GameObject pressCanvas;
    public GameObject paintingCanvas;
    public WallPainter wallPainter;

    [Header("Player")]
    public GameObject player;
    public FirstPersonController playerController;
    public CharacterController characterController;

    private bool playerNear = false;
    private bool sentado = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalFOV;
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        pressCanvas.SetActive(false);
        paintingCanvas.SetActive(false);
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (playerNear && !sentado && Input.GetKeyDown(KeyCode.F))
            Sentarse();
        else if (sentado && Input.GetKeyDown(KeyCode.F))
            Levantarse();
    }

    void Sentarse()
    {
        sentado = true;
        pressCanvas.SetActive(false);

        originalPosition = player.transform.position;
        originalRotation = player.transform.rotation;

        characterController.enabled = false;
        player.transform.position = puntoSentado.position;
        player.transform.rotation = Quaternion.Euler(0, puntoSentado.eulerAngles.y, 0);

        // Bloquear TODO el input del player incluyendo cámara
        playerController.MoveSpeed = 0f;
        playerController.SprintSpeed = 0f;
        playerController.RotationSpeed = 0f;

        // Desactivar el componente completo del FirstPersonController
        // para que no procese ningún input de cámara
        playerController.enabled = false;

        // Forzar rotación de la cámara
        if (virtualCamera != null)
        {
            virtualCamera.transform.rotation = Quaternion.Euler(
                0, puntoSentado.eulerAngles.y, 0
            );
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        paintingCanvas.SetActive(true);
        wallPainter.enabled = true;

        originalFOV = Camera.main.fieldOfView;
        Camera.main.fieldOfView = 80f;
    }

    void Levantarse()
    {
        sentado = false;
        paintingCanvas.SetActive(false);

        // Reactivar el FirstPersonController
        playerController.enabled = true;
        playerController.MoveSpeed = 4f;
        playerController.SprintSpeed = 6f;
        playerController.RotationSpeed = 1f;

        player.transform.position = originalPosition;
        player.transform.rotation = originalRotation;
        characterController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        wallPainter.enabled = false;
        Camera.main.fieldOfView = originalFOV;

        if (playerNear)
            pressCanvas.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (!sentado)
                pressCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            pressCanvas.SetActive(false);
        }
    }
}