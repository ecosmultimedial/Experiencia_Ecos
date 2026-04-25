using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtapaCentralManager : MonoBehaviour
{
    [Header("Primera visita")]
    public GameObject canvasBienvenida;
    public AudioSource vozEnOff;

private static bool primeraVez = true;

    private void Start()
    {
        canvasBienvenida.SetActive(false);

        if (primeraVez)
        {
            MostrarBienvenida();
        }
    }

    private void MostrarBienvenida()
    {
        canvasBienvenida.SetActive(true);
        vozEnOff.Play();
        primeraVez = false;

        Invoke(nameof(OcultarBienvenida), vozEnOff.clip.length);
    }

    private void OcultarBienvenida()
    {
        canvasBienvenida.SetActive(false);
    }

}