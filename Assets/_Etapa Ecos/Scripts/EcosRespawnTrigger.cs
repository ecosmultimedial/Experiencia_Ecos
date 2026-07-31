using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EcosRespawnTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El player (generalmente tiene CharacterController)")]
    public CharacterController playerCharacterController;

    [Tooltip("Si está vacío, buscará automáticamente el player")]
    public Transform playerTransform;

    [Header("Configuración")]
    [Tooltip("¿Debería haber un pequeño delay antes de resapawnear?")]
    public float delayRespawn = 0.5f;

    private bool yaUsado = false;

    void Start()
    {
        // Si no asignaste el player manualmente, búscalo automáticamente
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (playerCharacterController == null)
        {
            playerCharacterController = playerTransform.GetComponent<CharacterController>();
        }

        // Forzar que sea trigger
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo funciona si toca al player
        if (!other.CompareTag("Player")) return;

        // Ejecutar respawn
        StartCoroutine(EjecutarRespawn());
    }

    private IEnumerator EjecutarRespawn()
    {
        // Pequeño delay para dar feedback
        yield return new WaitForSeconds(delayRespawn);

        // Obtener la última posición segura del manager
        Transform posSigura = EcosCheckpointManager.instancia.ObtenerUltimaPosSigura();

        if (posSigura != null)
        {
            // Desactivar el CharacterController temporalmente
            if (playerCharacterController != null)
                playerCharacterController.enabled = false;

            // Mover el player a la posición segura
            playerTransform.position = posSigura.position;
            playerTransform.rotation = posSigura.rotation;

            // Reactivar el CharacterController
            if (playerCharacterController != null)
                playerCharacterController.enabled = true;

            Debug.Log($"[Respawn] Player resapawneado en: {posSigura.name}");
        }
        else
        {
            Debug.LogError("[Respawn] No se encontró posición de respawn válida.");
        }
    }
}