using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalReturnStage : MonoBehaviour
{
    public string nombreEscena = "Etapa Central";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("etapa central");
        }
    }
}