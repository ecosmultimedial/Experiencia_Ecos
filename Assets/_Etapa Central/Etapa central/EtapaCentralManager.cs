using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
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

    [Header("Cinemática de bienvenida (solo primera vez, desde Desconexion)")]
    public GameObject playerMeshVisual;      // el hijo con el mesh/cápsula visible
    public FirstPersonController fpsController;
    public CinemachineVirtualCamera vCamIntro;
    public CinemachineVirtualCamera vCamJugador;
    public Animation introAnimation;         // el componente Animation de CM_IntroPaneo
    public AudioSource audioBienvenida;
    public CanvasGroup panelNegroFade;
    public float fadeDuration = 1.5f;

    [Tooltip("Delay antes de extender el puente a Interior cuando es la primera visita (mientras dura la cinemática).")]
    public float delayPuenteInicial = 13f;

    [Header("Fade blanco de entrada (continuación del blanco de Desconexion)")]
    public CanvasGroup panelBlancoFade;
    public float fadeBlancoDuration = 1f;

    private void Start()
    {
        PosicionarPlayerSegunOrigen();
        DesplegarPuenteSegunOrigen();
        ActualizarSkybox();
        ActivarTriggerSegunProgreso();

        string origen = PlayerPrefs.GetString("OrigenEscena", "Desconexion");
        if (origen == "Desconexion")
        {
            StartCoroutine(IntroCinematicaBienvenida());
        }
    }

    private IEnumerator IntroCinematicaBienvenida()
    {
        if (playerMeshVisual != null) playerMeshVisual.SetActive(false);
        if (fpsController != null) fpsController.enabled = false;

        vCamIntro.Priority = 20;
        vCamJugador.Priority = 10;

        if (introAnimation != null) introAnimation.Play();
        if (audioBienvenida != null) audioBienvenida.Play();

        // Continuación del blanco heredado de Desconexión: se retira suavemente
        if (panelBlancoFade != null)
            yield return StartCoroutine(FadeCanvasGenerico(panelBlancoFade, 1f, 0f, fadeBlancoDuration));

        if (audioBienvenida != null)
            yield return new WaitForSeconds(audioBienvenida.clip.length);

        yield return StartCoroutine(FadeCanvasFadeIntro(0f, 1f));

        vCamJugador.Priority = 30;
        vCamIntro.Priority = 0;

        if (playerMeshVisual != null) playerMeshVisual.SetActive(true);
        if (fpsController != null) fpsController.enabled = true;

        yield return StartCoroutine(FadeCanvasFadeIntro(1f, 0f));
    }

    private IEnumerator FadeCanvasGenerico(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        cg.alpha = from;
        cg.blocksRaycasts = from > 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
        cg.blocksRaycasts = to > 0f;
    }

    private IEnumerator FadeCanvasFadeIntro(float from, float to)
    {
        float elapsed = 0f;
        panelNegroFade.alpha = from;
        panelNegroFade.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelNegroFade.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        panelNegroFade.alpha = to;
        if (to <= 0f) panelNegroFade.blocksRaycasts = false;
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
                if (puenteInterior != null) StartCoroutine(ExtenderPuenteConDelay(puenteInterior, delayPuenteInicial));
                break;
        }
    }

    private IEnumerator ExtenderPuenteConDelay(PuenteExtensible puente, float delay)
    {
        yield return new WaitForSeconds(delay);
        puente.ExtenderPuente();
    }
    private void ExtenderInstant(PuenteExtensible puente)
    {
        if (puente == null) return;
        puente.transform.localPosition = puente.posicionExtendida;
    }
}