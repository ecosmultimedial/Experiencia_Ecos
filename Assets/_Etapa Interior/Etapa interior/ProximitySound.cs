using UnityEngine;

public class ProximitySound : MonoBehaviour
{
    public AudioSource audioSource;
    public float activationRange = 4f;

    private Transform player;
    private bool isPlaying = false;

    void Start()
    {
        // Busca el player automáticamente, sin necesidad de asignarlo en el Inspector
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= activationRange && !isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
        else if (distance > activationRange && isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
