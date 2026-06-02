using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerEscalera : MonoBehaviour
{
    public enum Accion { IrAB, IrAA, Toggle }

    [Header("Configuración del trigger")]
    [Tooltip("La escalera que este trigger va a controlar")]
    public EscaleraRotatoria escalera;

    [Tooltip("Qué acción ejecuta al entrar el player")]
    public Accion accion = Accion.IrAB;

    [Tooltip("Si está activo, el trigger solo funciona una vez")]
    public bool unSoloUso = true;

    private bool yaUsado = false;

    void Reset()
    {
        // Al agregar el script, fuerza el collider como trigger
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (unSoloUso && yaUsado) return;
        if (escalera == null)
        {
            Debug.LogWarning($"[{name}] No tiene asignada una escalera.");
            return;
        }

        switch (accion)
        {
            case Accion.IrAB: escalera.RotarHaciaB(); break;
            case Accion.IrAA: escalera.RotarHaciaA(); break;
            case Accion.Toggle: escalera.Toggle(); break;
        }

        yaUsado = true;
    }
}
