using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerAnimacionEstacion : MonoBehaviour
{
    [Header("Animación de los objetos")]
    [Tooltip("Animators de los objetos a animar. Deben estar DESACTIVADOS al inicio.")]
    public Animator[] animatorsObjetos;

    [Tooltip("Duración aproximada de la animación más larga, en segundos")]
    public float duracionAnimacion = 2f;

    [Header("Popup")]
    public PopupContinuar popup;
    public Sprite imagenCartel;

    [Header("Después del popup")]
    public GameObject[] objetosAActivar;
    public GameObject[] objetosADesactivar;

    [Header("Bloqueo del player")]
    public MonoBehaviour scriptMovimientoPlayer;

    [Header("Checkpoint")]
    [Tooltip("Número del cubículo (1=FLOR, 2=STICKERS, 4=TRAMAS)")]
    public int numeroCubiculo = 0;

    [Header("Otros")]
    public bool unSoloUso = true;

    private bool yaUsado = false;

    void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (unSoloUso && yaUsado) return;

        yaUsado = true;
        StartCoroutine(SecuenciaEstacion());
    }

    private IEnumerator SecuenciaEstacion()
    {
        // 1. Encender todos los Animators → las animaciones arrancan en simultáneo
        foreach (var anim in animatorsObjetos)
        {
            if (anim != null) anim.enabled = true;
        }

        // 2. Esperar a que termine la más larga
        yield return new WaitForSeconds(duracionAnimacion);

        // 3. Bloquear al player
        if (scriptMovimientoPlayer != null) scriptMovimientoPlayer.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 4. Mostrar popup
        if (popup != null)
            popup.Mostrar(imagenCartel, OnPopupCerrado);
        else
            OnPopupCerrado();
    }

    private void OnPopupCerrado()
    {
        if (scriptMovimientoPlayer != null) scriptMovimientoPlayer.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (var obj in objetosAActivar)
            if (obj != null) obj.SetActive(true);
        foreach (var obj in objetosADesactivar)
            if (obj != null) obj.SetActive(false);

        // ✨ REGISTRAR CHECKPOINT
        if (numeroCubiculo > 0)
        {
            EcosCheckpointManager.instancia.RegistrarCheckpoint(numeroCubiculo);
        }
    }
}