using UnityEngine;

public class ProximityUI : MonoBehaviour
{
    public GameObject uiCanvas;

    void Start()
    {
        Debug.Log("ProximityUI iniciado en: " + gameObject.name);
        if (uiCanvas != null)
            uiCanvas.SetActive(false);
        else
            Debug.Log("ERROR: uiCanvas no asignado");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger tocado por: " + other.gameObject.name + " tag: " + other.tag);
        if (other.CompareTag("Player"))
            uiCanvas.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            uiCanvas.SetActive(false);
    }
}