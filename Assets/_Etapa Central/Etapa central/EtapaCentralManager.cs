using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtapaCentralManager : MonoBehaviour
{
    [Header("Primera visita")]
    public GameObject canvasBienvenida;
    public AudioSource vozEnOff;

    private void Start()
    {
        canvasBienvenida.SetActive(false);

        // Solo suena si viene desde la etapa desconexion (primera vez)
        if (PlayerPrefs.GetInt("VozCentralReproducida", 0) == 0)
        {
            MostrarBienvenida();
            PlayerPrefs.SetInt("VozCentralReproducida", 1);
            PlayerPrefs.Save();
        }
    }

    private void MostrarBienvenida()
    {
        canvasBienvenida.SetActive(true);
        vozEnOff.Play();
        Invoke(nameof(OcultarBienvenida), vozEnOff.clip.length);
    }

    private void OcultarBienvenida()
    {
        canvasBienvenida.SetActive(false);
    }
}