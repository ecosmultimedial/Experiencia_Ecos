using System.Collections;
using UnityEngine;

public class ControlParedPortal : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource vozPortal;
    public GameObject paredInvisible;

    private bool vozReproducida = false;
    private bool vozTerminada = false;

    void Update()
    {
        if (vozPortal == null || paredInvisible == null) return;

        // Detectar cuando arranca la voz
        if (!vozReproducida && vozPortal.isPlaying)
            vozReproducida = true;

        // Desactivar pared solo cuando la voz terminó de reproducirse al menos una vez
        if (!vozTerminada && vozReproducida && !vozPortal.isPlaying)
        {
            vozTerminada = true;
            paredInvisible.SetActive(false);
        }
    }
}