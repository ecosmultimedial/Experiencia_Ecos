using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PuenteExtensible : MonoBehaviour
{
    [Header("Posiciones (locales, relativas al padre)")]
    [Tooltip("Posicion retraida del puente (estado inicial). Usar valores del Transform del Inspector.")]
    public Vector3 posicionRetraida;
    [Tooltip("Posicion extendida del puente (pegado al portal). Usar valores del Transform del Inspector.")]
    public Vector3 posicionExtendida;
    [Header("Animacion")]
    [Tooltip("Duracion de la animacion en segundos.")]
    public float duracionAnimacion = 2.5f;
    [Tooltip("Curva de aceleracion. Dejala default para suave.")]
    public AnimationCurve curva = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Header("Inicio")]
    [Tooltip("Si esta marcado, el puente arranca extendido (para puentes ya activados).")]
    public bool empiezaExtendido = false;
    [Header("Borde de seguridad")]
    [Tooltip("Collider que bloquea el vacio cuando el puente esta retraido. Se desactiva al extenderse.")]
    public GameObject bordeSeguridad;

    [Header("Sonido")]
    [Tooltip("Sonido que se reproduce cuando el puente se despliega.")]
    public AudioClip sonidoDespliegue;

    private AudioSource audioSource;
    private bool yaExtendido = false;

    private void Awake()
    {
        transform.localPosition = empiezaExtendido ? posicionExtendida : posicionRetraida;
        yaExtendido = empiezaExtendido;
        if (bordeSeguridad != null) bordeSeguridad.SetActive(!empiezaExtendido);

        audioSource = GetComponent<AudioSource>();
    }

    public void ExtenderPuente()
    {
        if (yaExtendido) return;
        yaExtendido = true;
        if (bordeSeguridad != null) bordeSeguridad.SetActive(false);

        if (audioSource != null && sonidoDespliegue != null)
            audioSource.PlayOneShot(sonidoDespliegue);

        StartCoroutine(AnimarExtension());
    }

    private IEnumerator AnimarExtension()
    {
        float t = 0f;
        Vector3 inicio = posicionRetraida;
        Vector3 fin = posicionExtendida;
        while (t < duracionAnimacion)
        {
            t += Time.deltaTime;
            float progreso = Mathf.Clamp01(t / duracionAnimacion);
            float eased = curva.Evaluate(progreso);
            transform.localPosition = Vector3.Lerp(inicio, fin, eased);
            yield return null;
        }
        transform.localPosition = fin;
    }
}