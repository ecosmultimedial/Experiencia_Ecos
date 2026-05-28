using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("VozCentralReproducida", 0);
        PlayerPrefs.SetInt("VisitoEtapaInterior", 0);
        PlayerPrefs.Save();
    }
}