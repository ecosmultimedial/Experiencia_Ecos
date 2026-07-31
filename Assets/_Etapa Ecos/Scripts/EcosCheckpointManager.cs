using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosCheckpointManager : MonoBehaviour
{
    [Header("Referencias a Checkpoints")]
    [Tooltip("Posición 0: Inicio")]
    public Transform checkpoint_Inicio;

    [Tooltip("Posición 1: Después de completar FLOR")]
    public Transform checkpoint_DespuesFLOR;

    [Tooltip("Posición 2: Después de completar STICKERS")]
    public Transform checkpoint_DespuesSTICKERS;

    [Tooltip("Posición 3: Después de completar VELA")]
    public Transform checkpoint_DespuesVELA;

    [Tooltip("Posición 4: Después de completar TRAMAS")]
    public Transform checkpoint_DespuesTRAMAS;

    [Tooltip("Posición 5: Fin Punto Quiebre (antes del portal)")]
    public Transform checkpoint_FinPuntoQuiebre;

    // Estado interno: qué cubículos han sido completados
    private bool[] cubiculosCompletados = new bool[5]; // 0-4 (Inicio no se cuenta)

    // Singleton
    public static EcosCheckpointManager instancia;

    void Awake()
    {
        // Crear singleton
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicializar: todos los cubículos empiezan sin completar
        for (int i = 0; i < cubiculosCompletados.Length; i++)
        {
            cubiculosCompletados[i] = false;
        }
    }

    /// <summary>
    /// Registra que un cubículo fue completado.
    /// cubiculoIndex: 1 (FLOR), 2 (STICKERS), 3 (VELA), 4 (TRAMAS)
    /// </summary>
    public void RegistrarCheckpoint(int cubiculoIndex)
    {
        if (cubiculoIndex > 0 && cubiculoIndex < cubiculosCompletados.Length + 1)
        {
            cubiculosCompletados[cubiculoIndex - 1] = true;
            Debug.Log($"[Checkpoint] Cubículo {cubiculoIndex} completado. Posición de respawn actualizada.");
        }
    }

    /// <summary>
    /// Devuelve la posición donde el player debería resapawnear
    /// </summary>
    public Transform ObtenerUltimaPosSigura()
    {
        // Recorrer de atrás hacia adelante para encontrar el último cubículo completado
        for (int i = cubiculosCompletados.Length - 1; i >= 0; i--)
        {
            if (cubiculosCompletados[i])
            {
                // Retornar la posición correspondiente
                switch (i)
                {
                    case 0: return checkpoint_DespuesFLOR;
                    case 1: return checkpoint_DespuesSTICKERS;
                    case 2: return checkpoint_DespuesVELA;
                    case 3: return checkpoint_DespuesTRAMAS;
                    case 4: return checkpoint_FinPuntoQuiebre;
                }
            }
        }

        // Si no hay ninguno completado, retornar Inicio
        return checkpoint_Inicio;
    }

    /// <summary>
    /// Imprime el estado actual de los checkpoints (para debug)
    /// </summary>
    public void ImprimirEstado()
    {
        Debug.Log("=== Estado de Checkpoints ===");
        Debug.Log($"FLOR completada: {cubiculosCompletados[0]}");
        Debug.Log($"STICKERS completada: {cubiculosCompletados[1]}");
        Debug.Log($"VELA completada: {cubiculosCompletados[2]}");
        Debug.Log($"TRAMAS completada: {cubiculosCompletados[3]}");
        Debug.Log($"FIN PUNTO QUIEBRE: {cubiculosCompletados[4]}");
    }
}
