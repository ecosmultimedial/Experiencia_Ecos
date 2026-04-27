using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalReturnStage : MonoBehaviour
{
    public int numeroEtapa;          // qué etapa se completó
    public string sceneName = "EtapaCentral";   // escena a la que vuelve

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // guarda el progreso
            PlayerPrefs.SetInt("EtapaCompletada", numeroEtapa);

            // carga la escena central
            SceneManager.LoadScene(sceneName);
        }
    }
}