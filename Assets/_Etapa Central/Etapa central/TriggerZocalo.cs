using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZocalo : MonoBehaviour
{
    public ZocaloTypewriter zocalo;
    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            zocalo.Activar();
        }
    }
}