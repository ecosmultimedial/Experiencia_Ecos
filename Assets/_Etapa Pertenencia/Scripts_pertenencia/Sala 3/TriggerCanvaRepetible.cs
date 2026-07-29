using UnityEngine;

public class TriggerCanvaRepetible : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canvas.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canvas.SetActive(false);
    }
}