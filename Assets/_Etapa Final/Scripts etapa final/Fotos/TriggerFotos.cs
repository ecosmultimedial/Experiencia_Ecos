using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFotos : MonoBehaviour
{
    public GestorFotos gestorFotos;
    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;
            gestorFotos.MostrarSiguienteGrupo();
        }
    }
}