using UnityEngine;

public class TuboMusical : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip[] sonidos; // Arrastrá los clips de la carpeta de este tubo

    [Header("UI")]
    public GameObject pressXCanvas; // Canvas con "X para activar / P para pausar"

    [Header("Distancia sonido")]
    public float maxDistance = 8f;
    public float minDistance = 1f;

    private AudioSource audioSource;
    private Transform player;
    private bool playerNear = false;
    private int sonidoActual = -1; // -1 = apagado
    private bool pausado = false;

    // Referencia al manager global para registrarse
    private TuboCentral tuboCentral;

    void Start()
    {
        // Crear AudioSource por script
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 0f;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Registrarse en el TuboCentral
        tuboCentral = FindObjectOfType<TuboCentral>();
        if (tuboCentral != null)
            tuboCentral.RegistrarTubo(this);

        if (pressXCanvas != null)
            pressXCanvas.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        // Control de volumen por distancia
        float distance = Vector3.Distance(player.position, transform.position);
        if (sonidoActual >= 0 && !pausado)
        {
            if (distance <= maxDistance)
            {
                float volume = 1f - Mathf.Clamp01(
                    (distance - minDistance) / (maxDistance - minDistance)
                );
                audioSource.volume = volume;
            }
            else
            {
                audioSource.volume = 0f;
            }
        }

        // Interacción
        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.X))
                CambiarSonido();

            if (Input.GetKeyDown(KeyCode.P))
                TogglePausa();
        }
    }

    void CambiarSonido()
    {
        sonidoActual++;
        if (sonidoActual >= sonidos.Length)
            sonidoActual = -1;

        audioSource.Stop();

        if (sonidoActual >= 0)
        {
            audioSource.clip = sonidos[sonidoActual];
            audioSource.loop = true;
            audioSource.Play();
            pausado = false;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }

    public void TogglePausa()
    {
        if (sonidoActual < 0) return;

        pausado = !pausado;
        if (pausado)
        {
            audioSource.Pause();
            audioSource.volume = 0f;
        }
        else
        {
            audioSource.UnPause();
        }
    }

    public void PausarDesdeGlobal()
    {
        if (sonidoActual < 0) return;
        pausado = true;
        audioSource.Pause();
        audioSource.volume = 0f;
    }

    public void ReanudarDesdeGlobal()
    {
        if (sonidoActual < 0) return;
        pausado = false;
        audioSource.UnPause();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (pressXCanvas != null)
                pressXCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            if (pressXCanvas != null)
                pressXCanvas.SetActive(false);
        }
    }
}