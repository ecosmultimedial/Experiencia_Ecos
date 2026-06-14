using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFinal : MonoBehaviour
{
    public FadeABlanco fadeABlanco;
    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;
            fadeABlanco.IniciarSecuenciaFinal();
        }
    }
}