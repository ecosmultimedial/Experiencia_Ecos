using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip footstepClip;
    public float stepInterval = 0.45f;

    private AudioSource audioSource;
    private CharacterController controller;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = footstepClip;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    void Update()
    {
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        bool isMoving = controller.enabled && controller.isGrounded && horizontalVelocity.magnitude > 0.1f;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                audioSource.Play();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Detener el sonido inmediatamente al parar
            if (audioSource.isPlaying)
                audioSource.Stop();
            stepTimer = 0f;
        }
    }
}