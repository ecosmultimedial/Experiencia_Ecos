using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CartelTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CartelActividad cartelActividad;

    [Header("Configuración")]
    [SerializeField] private float tiempoEspera = 10f;
    [SerializeField] private string tagJugador = "Player";

    private Coroutine cuentaRegresivaCoroutine;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagJugador)) return;
        if (cartelActividad == null || cartelActividad.EstaCompletada) return;

        if (cuentaRegresivaCoroutine != null)
            StopCoroutine(cuentaRegresivaCoroutine);

        cuentaRegresivaCoroutine = StartCoroutine(CuentaRegresiva());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(tagJugador)) return;

        if (cuentaRegresivaCoroutine != null)
        {
            StopCoroutine(cuentaRegresivaCoroutine);
            cuentaRegresivaCoroutine = null;
        }

        if (cartelActividad != null)
            cartelActividad.OcultarBoton();
    }

    private IEnumerator CuentaRegresiva()
    {
        yield return new WaitForSeconds(tiempoEspera);
        cartelActividad.MostrarBoton();
        cuentaRegresivaCoroutine = null;
    }
}