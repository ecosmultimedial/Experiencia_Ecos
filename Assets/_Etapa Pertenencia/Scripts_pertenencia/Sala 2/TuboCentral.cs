using System.Collections.Generic;
using UnityEngine;

public class TuboCentral : MonoBehaviour
{
    [Header("UI")]
    public GameObject instruccionesCanvas;

    private List<TuboMusical> tubos = new List<TuboMusical>();
    private bool playerNear = false;
    private bool todosPausados = false;

    public void RegistrarTubo(TuboMusical tubo)
    {
        if (!tubos.Contains(tubo))
            tubos.Add(tubo);
    }

    void Start()
    {
        if (instruccionesCanvas != null)
            instruccionesCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.P))
            TogglePausaGlobal();
    }

    void TogglePausaGlobal()
    {
        todosPausados = !todosPausados;

        foreach (TuboMusical tubo in tubos)
        {
            if (todosPausados)
                tubo.PausarDesdeGlobal();
            else
                tubo.ReanudarDesdeGlobal();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (instruccionesCanvas != null)
                instruccionesCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            if (instruccionesCanvas != null)
                instruccionesCanvas.SetActive(false);
        }
    }
}