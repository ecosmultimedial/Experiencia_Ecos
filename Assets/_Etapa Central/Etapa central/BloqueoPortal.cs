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

        bool etapaCompletada = PlayerPrefs.GetInt("Etapa" + nombreEtapa + "Completada", 0) == 1;
        miCollider.enabled = etapaCompletada;
        if (paredBloqueo != null) paredBloqueo.SetActive(etapaCompletada);
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