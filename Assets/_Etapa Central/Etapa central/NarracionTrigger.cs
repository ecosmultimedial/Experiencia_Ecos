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

    private Collider miCollider;
    private bool yaReproducida;
    private Coroutine fadeEnCurso;

    private void Awake()
    {
        // Se resuelve en Awake (no en Start) para garantizar que el collider
        // ya este listo antes de que EtapaCentralManager decida activarlo/desactivarlo en su propio Start.
        miCollider = GetComponent<Collider>();
        yaReproducida = PlayerPrefs.GetInt(ClavePlayerPref(), 0) == 1;

        if (zocalo != null)
        {
            zocalo.alpha = 0f;
            zocalo.gameObject.SetActive(false);
            zocalo.interactable = false;
            zocalo.blocksRaycasts = false;
        }

        if (miCollider != null) miCollider.enabled = false;
    }

    private string ClavePlayerPref()
    {
        return "Narracion" + idNarracion + "Reproducida";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (yaReproducida) return;

        Reproducir();
    }

    private void Reproducir()
    {
        yaReproducida = true;
        PlayerPrefs.SetInt(ClavePlayerPref(), 1);
        PlayerPrefs.Save();

        if (zocalo != null)
        {
            zocalo.gameObject.SetActive(true);
            if (zocaloTexto != null) zocaloTexto.LimpiarTexto();
            IniciarFade(1f, duracionFadeIn, alTerminar: () =>
            {
                if (zocaloTexto != null) zocaloTexto.Activar();
            });
        }

        if (vozNarracion != null)
        {
            vozNarracion.Play();
            float duracion = vozNarracion.clip != null ? vozNarracion.clip.length : 3f;
            Invoke(nameof(OcultarZocalo), duracion);
        }
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

        // Mientras el zocalo esta apareciendo, que se pueda interactuar si tuviera botones.
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
        if (yaReproducida) return; // ya se reprodujo en una sesion anterior, no hace falta activarlo
        if (miCollider != null) miCollider.enabled = true;
    }

    public void Desactivar()
    {
        if (miCollider != null) miCollider.enabled = false;
    }
}