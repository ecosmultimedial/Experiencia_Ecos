using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtapaCentralManager : MonoBehaviour
{
    [Header("Triggers de narracion (uno por etapa siguiente)")]
    [Tooltip("Trigger cerca del portal de Interior. Tambien cumple el rol de bienvenida inicial.")]
    public NarracionTrigger triggerInterior;
    public NarracionTrigger triggerAfectiva;
    public NarracionTrigger triggerPertenencia;
    public NarracionTrigger triggerEcos;

    [Header("Spawn del player segun origen")]
    public Transform player;
    public Transform spawnDesdeInterior;
    public Transform spawnDesdeAfectiva;
    public Transform spawnDesdePertenencia;
    public Transform spawnDesdeEcos;

    [Header("Puentes")]
    public PuenteExtensible puenteInterior;
    public PuenteExtensible puenteAfectiva;
    public PuenteExtensible puentePertenencia;
    public PuenteExtensible puenteEcos;

    [Header("Skyboxes")]
    [Tooltip("Indice 0 = ninguna etapa completada, indice 4 = todas completadas.")]
    public Material[] skyboxes = new Material[5];

    private void Start()
    {
        PosicionarPlayerSegunOrigen();
        DesplegarPuenteSegunOrigen();
        ActualizarSkybox();
        ActivarTriggerSegunProgreso();
    }

    private void ActivarTriggerSegunProgreso()
    {
        DesactivarTodosLosTriggers();

        bool interiorListo = PlayerPrefs.GetInt("EtapaInteriorCompletada", 0) == 1;
        bool afectivaListo = PlayerPrefs.GetInt("EtapaAfectivaCompletada", 0) == 1;
        bool pertenenciaListo = PlayerPrefs.GetInt("EtapaPertenenciaCompletada", 0) == 1;
        bool ecosListo = PlayerPrefs.GetInt("EtapaEcosCompletada", 0) == 1;

        if (ecosListo)
        {
            // Todas las etapas completadas, no queda ninguna etapa siguiente que anunciar.
            return;
        }
        else if (pertenenciaListo)
        {
            ActivarTrigger(triggerEcos);
        }
        else if (afectivaListo)
        {
            ActivarTrigger(triggerPertenencia);
        }
        else if (interiorListo)
        {
            ActivarTrigger(triggerAfectiva);
        }
        else
        {
            ActivarTrigger(triggerInterior);
        }
    }

    private void DesactivarTodosLosTriggers()
    {
        if (triggerInterior != null) triggerInterior.Desactivar();
        if (triggerAfectiva != null) triggerAfectiva.Desactivar();
        if (triggerPertenencia != null) triggerPertenencia.Desactivar();
        if (triggerEcos != null) triggerEcos.Desactivar();
    }

    private void ActivarTrigger(NarracionTrigger trigger)
    {
        if (trigger != null) trigger.Activar();
    }

    private void ActualizarSkybox()
    {
        int etapasCompletadas = 0;
        if (PlayerPrefs.GetInt("EtapaInteriorCompletada", 0) == 1) etapasCompletadas++;
        if (PlayerPrefs.GetInt("EtapaAfectivaCompletada", 0) == 1) etapasCompletadas++;
        if (PlayerPrefs.GetInt("EtapaPertenenciaCompletada", 0) == 1) etapasCompletadas++;
        if (PlayerPrefs.GetInt("EtapaEcosCompletada", 0) == 1) etapasCompletadas++;

        if (skyboxes != null && etapasCompletadas < skyboxes.Length && skyboxes[etapasCompletadas] != null)
        {
            RenderSettings.skybox = skyboxes[etapasCompletadas];
            DynamicGI.UpdateEnvironment();
        }
    }

    private void PosicionarPlayerSegunOrigen()
    {
        string origen = PlayerPrefs.GetString("OrigenEscena", "Desconexion");
        Transform spawn = null;

        switch (origen)
        {
            case "Interior": spawn = spawnDesdeInterior; break;
            case "Afectiva": spawn = spawnDesdeAfectiva; break;
            case "Pertenencia": spawn = spawnDesdePertenencia; break;
            case "Ecos": spawn = spawnDesdeEcos; break;
        }

        if (spawn == null || player == null) return;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = spawn.position;
        player.rotation = spawn.rotation;

        if (cc != null) cc.enabled = true;
    }

    private void DesplegarPuenteSegunOrigen()
    {
        string origen = PlayerPrefs.GetString("OrigenEscena", "Desconexion");

        switch (origen)
        {
            case "Ecos":
                ExtenderInstant(puenteEcos);
                ExtenderInstant(puentePertenencia);
                ExtenderInstant(puenteAfectiva);
                ExtenderInstant(puenteInterior);
                break;
            case "Pertenencia":
                ExtenderInstant(puentePertenencia);
                ExtenderInstant(puenteAfectiva);
                ExtenderInstant(puenteInterior);
                if (puenteEcos != null) puenteEcos.ExtenderPuente();
                break;
            case "Afectiva":
                ExtenderInstant(puenteAfectiva);
                ExtenderInstant(puenteInterior);
                if (puentePertenencia != null) puentePertenencia.ExtenderPuente();
                break;
            case "Interior":
                ExtenderInstant(puenteInterior);
                if (puenteAfectiva != null) puenteAfectiva.ExtenderPuente();
                break;
            case "Desconexion":
                if (puenteInterior != null) puenteInterior.ExtenderPuente();
                break;
        }
    }

    private void ExtenderInstant(PuenteExtensible puente)
    {
        if (puente == null) return;
        puente.transform.localPosition = puente.posicionExtendida;
    }
}