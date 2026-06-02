using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarBotonContinuar : MonoBehaviour
{
    [SerializeField] private Button botonContinuar;
    [SerializeField] private float tiempoEspera = 10f;
    private Coroutine contadorActual;
    private bool yaContinuado = false;

    void Start()
    {
        botonContinuar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (botonContinuar.gameObject.activeSelf &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Continuar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (yaContinuado) return;

        if (other.CompareTag("Player"))
        {
            contadorActual = StartCoroutine(MostrarBotonDespuesDeTiempo());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (yaContinuado) return;

        if (other.CompareTag("Player"))
        {
            if (contadorActual != null)
            {
                StopCoroutine(contadorActual);
                contadorActual = null;
            }
            botonContinuar.gameObject.SetActive(false);
        }
    }

    IEnumerator MostrarBotonDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoEspera);
        botonContinuar.gameObject.SetActive(true);
    }

    public void Continuar()
    {
        Debug.Log("Continuar presionado");
        yaContinuado = true;
        botonContinuar.gameObject.SetActive(false);
    }
}