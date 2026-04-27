using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoPortalCentral : MonoBehaviour
{
    public GameObject paredInvisible;
    public GameObject canvasAviso;

    void Start()
    {
        paredInvisible.SetActive(false);
        canvasAviso.SetActive(false);

        if (PlayerPrefs.GetInt("VisitoEtapaInterior", 0) == 1)
        {
            paredInvisible.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("VisitoEtapaInterior", 0) == 1)
        {
            canvasAviso.SetActive(true);
        }
    }
}