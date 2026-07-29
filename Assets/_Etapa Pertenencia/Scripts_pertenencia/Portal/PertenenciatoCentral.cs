using UnityEngine;
using UnityEngine.SceneManagement;

public class PertenenciatoCentral : MonoBehaviour
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