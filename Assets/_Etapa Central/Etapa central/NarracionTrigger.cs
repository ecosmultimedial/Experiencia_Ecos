using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarracionTrigger : MonoBehaviour
{
    [Header("Identificacion")]
    [Tooltip("Nombre de la etapa que esta narracion corresponde (Interior, Afectiva, Pertenencia, Ecos). Debe ser unico.")]
    public string idNarracion;

    [Header("Contenido a reproducir")]
    [Tooltip("Debe tener un componente CanvasGroup para poder hacer fade.")]
    public CanvasGroup zocalo;
    [Tooltip("Opcional. Si esta asignado, el texto arranca a tipear recien cuando termina el fade in.")]
    public ZocaloTypewriter zocaloTexto;
    public AudioSource vozNarracion;

    [Header("Fade")]
    public float duracionFadeIn = 0.5f;
    public float duracionFadeOut = 0.5f;

    [Header("Permanencia luego de terminar de escribir")]
    public float tiempoVisibleAlTerminar = 4f;

    [Header("Bloqueo fisico durante el audio")]
    [Tooltip("GameObject con collider solido (no trigger) que bloquea el paso al portal mientras dura el audio.")]
    public GameObject paredBloqueoAudio;

    private Collider miCollider;
    private Coroutine fadeEnCurso;

    private void Awake()
    {
        miCollider = GetComponent<Collider>();

        if (zocalo != null)
        {
            zocalo.alpha = 0f;
            zocalo.gameObject.SetActive(false);
            zocalo.interactable = false;
            zocalo.blocksRaycasts = false;
        }

        if (paredBloqueoAudio != null) paredBloqueoAudio.SetActive(false);

        if (miCollider != null) miCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Reproducir();
    }

    private void Reproducir()
    {
        // Si se vuelve a entrar al trigger, cancelamos cualquier cuenta pendiente de la vez anterior
        CancelInvoke(nameof(OcultarZocalo));
        CancelInvoke(nameof(DesbloquearPaso));

        if (paredBloqueoAudio != null) paredBloqueoAudio.SetActive(true);

        if (vozNarracion != null)
        {
            vozNarracion.Stop();
            vozNarracion.Play();

            float duracionAudio = vozNarracion.clip != null ? vozNarracion.clip.length : 3f;
            Invoke(nameof(DesbloquearPaso), duracionAudio);
        }
        else
        {
            DesbloquearPaso();
        }

        if (zocalo != null)
        {
            zocalo.gameObject.SetActive(true);
            if (zocaloTexto != null) zocaloTexto.LimpiarTexto();

            IniciarFade(1f, duracionFadeIn, alTerminar: () =>
            {
                if (zocaloTexto != null)
                {
                    zocaloTexto.Activar(() =>
                    {
                        Invoke(nameof(OcultarZocalo), tiempoVisibleAlTerminar);
                    });
                }
            });
        }
    }

    private void DesbloquearPaso()
    {
        if (paredBloqueoAudio != null) paredBloqueoAudio.SetActive(false);
    }

    private void OcultarZocalo()
    {
        if (zocaloTexto != null) zocaloTexto.DetenerEscritura();
        if (zocalo == null) return;
        IniciarFade(0f, duracionFadeOut, desactivarAlTerminar: true);
    }

    private void IniciarFade(float alphaDestino, float duracion, bool desactivarAlTerminar = false, System.Action alTerminar = null)
    {
        if (fadeEnCurso != null) StopCoroutine(fadeEnCurso);
        fadeEnCurso = StartCoroutine(FadeCanvasGroup(alphaDestino, duracion, desactivarAlTerminar, alTerminar));
    }

    private IEnumerator FadeCanvasGroup(float alphaDestino, float duracion, bool desactivarAlTerminar, System.Action alTerminar)
    {
        float alphaInicial = zocalo.alpha;
        float tiempo = 0f;

        zocalo.interactable = alphaDestino > 0f;
        zocalo.blocksRaycasts = alphaDestino > 0f;

        if (duracion <= 0f)
        {
            zocalo.alpha = alphaDestino;
        }
        else
        {
            while (tiempo < duracion)
            {
                tiempo += Time.deltaTime;
                zocalo.alpha = Mathf.Lerp(alphaInicial, alphaDestino, tiempo / duracion);
                yield return null;
            }
            zocalo.alpha = alphaDestino;
        }

        if (desactivarAlTerminar)
        {
            zocalo.gameObject.SetActive(false);
        }

        alTerminar?.Invoke();
        fadeEnCurso = null;
    }

    public void Activar()
    {
        if (miCollider != null) miCollider.enabled = true;
    }

    public void Desactivar()
    {
        if (miCollider != null) miCollider.enabled = false;
    }
}