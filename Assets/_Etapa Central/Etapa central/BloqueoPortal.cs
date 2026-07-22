using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoPortal : MonoBehaviour
{
    [Header("Configuracion")]
    public string nombreEtapa;
    public GameObject canvasCartel;

    [Header("Pared fisica")]
    public GameObject paredBloqueo;

    private Collider miCollider;

    private void Start()
    {
        miCollider = GetComponent<Collider>();
        if (canvasCartel != null) canvasCartel.SetActive(false);

        bool esElTurnoDeEstaEtapa = EsElTurnoDeEstaEtapa();

        miCollider.enabled = !esElTurnoDeEstaEtapa;
        if (paredBloqueo != null) paredBloqueo.SetActive(!esElTurnoDeEstaEtapa);
    }

    private bool EsElTurnoDeEstaEtapa()
    {
        bool interiorListo = PlayerPrefs.GetInt("EtapaInteriorCompletada", 0) == 1;
        bool afectivaListo = PlayerPrefs.GetInt("EtapaAfectivaCompletada", 0) == 1;
        bool pertenenciaListo = PlayerPrefs.GetInt("EtapaPertenenciaCompletada", 0) == 1;
        bool ecosListo = PlayerPrefs.GetInt("EtapaEcosCompletada", 0) == 1;

        string etapaActual;
        if (ecosListo) etapaActual = "";               // ya completó todo, ninguna es "la actual"
        else if (pertenenciaListo) etapaActual = "Ecos";
        else if (afectivaListo) etapaActual = "Pertenencia";
        else if (interiorListo) etapaActual = "Afectiva";
        else etapaActual = "Interior";

        return nombreEtapa == etapaActual;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canvasCartel != null)
        {
            canvasCartel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CerrarCartel()
    {
        if (canvasCartel != null) canvasCartel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}