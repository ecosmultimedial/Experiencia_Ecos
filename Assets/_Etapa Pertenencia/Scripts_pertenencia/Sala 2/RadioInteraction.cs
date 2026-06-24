using UnityEngine;

public class RadioInteraction : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip sinSenal;
    public AudioClip[] canciones;

    [Header("UI")]
    public GameObject pressXCanvas;

    [Header("Distancia")]
    public float maxDistance = 10f;  // Ajustá este número libremente
    public float minDistance = 2f;   // Distancia donde suena al máximo

    private AudioSource audioSource;
    private int cancionActual = -1;
    private bool playerNear = false;
    private Transform player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sinSenal;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // Lo manejamos nosotros por script
        audioSource.Play();
        pressXCanvas.SetActive(false);

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Calcular volumen por distancia manualmente
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= maxDistance)
        {
            // Volumen va de 1 a 0 entre minDistance y maxDistance
            float volume = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
            audioSource.volume = volume;
        }
        else
        {
            audioSource.volume = 0f;
        }

        if (playerNear && Input.GetKeyDown(KeyCode.X))
        {
            CambiarCancion();
        }
    }

    void CambiarCancion()
    {
        cancionActual++;
        if (cancionActual >= canciones.Length)
            cancionActual = -1;

        audioSource.Stop();
        audioSource.clip = cancionActual == -1 ? sinSenal : canciones[cancionActual];
        audioSource.loop = true;
        audioSource.Play();
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