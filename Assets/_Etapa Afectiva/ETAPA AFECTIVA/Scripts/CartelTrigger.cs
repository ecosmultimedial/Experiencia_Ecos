using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CartelTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CartelActividad cartelActividad;
    [SerializeField] private ZocaloCarga zocaloCarga;

    [Header("Configuración")]
    [SerializeField] private float tiempoEspera = 10f;
    [SerializeField] private string tagJugador = "Player";

    private Coroutine cuentaRegresivaCoroutine;
    private bool cuentaIniciada = false;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagJugador)) return;
        if (cartelActividad == null || cartelActividad.EstaCompletada) return;
        if (cuentaIniciada) return; // <- si ya arrancó, no hacer nada

        cuentaIniciada = true;
        cuentaRegresivaCoroutine = StartCoroutine(CuentaRegresiva());
    }

    // OnTriggerExit ya no cancela nada

    private IEnumerator CuentaRegresiva()
    {
        cartelActividad.BloquearMovimiento(); // <- bloquear al entrar

        if (zocaloCarga != null)
            zocaloCarga.Mostrar();

        yield return new WaitForSeconds(tiempoEspera);

        if (zocaloCarga != null)
            yield return StartCoroutine(zocaloCarga.OcultarYEsperar());

        cartelActividad.MostrarBoton();
        cuentaRegresivaCoroutine = null;
    }
}