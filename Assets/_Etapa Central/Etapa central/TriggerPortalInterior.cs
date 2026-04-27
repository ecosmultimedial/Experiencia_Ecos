using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerPortalInterior : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("VisitoEtapaInterior", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Etapa_interior");
        }
    }
}