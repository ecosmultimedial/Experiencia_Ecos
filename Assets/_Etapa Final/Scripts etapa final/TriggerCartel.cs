using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCartel : MonoBehaviour
{
    public CartelRespuesta gestorCarteles;
    public bool esCierre = false;
    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;
            if (esCierre)
                gestorCarteles.CerrarUltimo();
            else
                gestorCarteles.MostrarSiguiente();
        }
    }
}