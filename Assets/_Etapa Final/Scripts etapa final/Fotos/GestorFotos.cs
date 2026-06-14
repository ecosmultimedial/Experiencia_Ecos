using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorFotos : MonoBehaviour
{
    [Header("Grupos de fotos")]
    public GrupoFotos[] grupos;

    [Header("Animación")]
    public float duracionFade = 1f;
    public float desplazamientoInicial = 5f;

    private int indiceActual = -1;

    void Start()
    {
        foreach (var grupo in grupos)
        {
            grupo.fotoIzquierda.gameObject.SetActive(false);
            grupo.fotoDerecha.gameObject.SetActive(false);
        }
    }

    void SetAlpha(Material mat, float alpha)
    {
        if (mat.HasProperty("_BaseColor"))
        {
            Color c = mat.GetColor("_BaseColor");
            c.a = alpha;
            mat.SetColor("_BaseColor", c);
        }
        if (mat.HasProperty("_Color"))
        {
            Color c = mat.color;
            c.a = alpha;
            mat.color = c;
        }
        if (mat.HasProperty("_EmissionColor"))
        {
            Color e = mat.GetColor("_EmissionColor");
            mat.SetColor("_EmissionColor", new Color(e.r * alpha, e.g * alpha, e.b * alpha, alpha));
        }
    }

    public void MostrarSiguienteGrupo()
    {
        indiceActual++;
        if (indiceActual < grupos.Length)
        {
            StartCoroutine(AnimarGrupo(grupos[indiceActual]));
        }
    }

    IEnumerator AnimarGrupo(GrupoFotos grupo)
    {
        grupo.fotoIzquierda.gameObject.SetActive(true);
        grupo.fotoDerecha.gameObject.SetActive(true);

        foreach (var r in grupo.fotoIzquierda.GetComponentsInChildren<Renderer>())
        {
            Material[] mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]);
                SetAlpha(mats[i], 0f);
            }
            r.materials = mats;
        }

        foreach (var r in grupo.fotoDerecha.GetComponentsInChildren<Renderer>())
        {
            Material[] mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]);
                SetAlpha(mats[i], 0f);
            }
            r.materials = mats;
        }

        Vector3 posOriginalIzq = grupo.fotoIzquierda.localPosition;
        Vector3 posOriginalDer = grupo.fotoDerecha.localPosition;

        grupo.fotoIzquierda.localPosition += Vector3.back * desplazamientoInicial;
        grupo.fotoDerecha.localPosition += Vector3.forward * desplazamientoInicial;

        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            float progreso = Mathf.SmoothStep(0f, 1f, t / duracionFade);

            grupo.fotoIzquierda.localPosition = Vector3.Lerp(
                posOriginalIzq + Vector3.back * desplazamientoInicial,
                posOriginalIzq, progreso);
            FadeFoto(grupo.fotoIzquierda, progreso);

            grupo.fotoDerecha.localPosition = Vector3.Lerp(
                posOriginalDer + Vector3.forward * desplazamientoInicial,
                posOriginalDer, progreso);
            FadeFoto(grupo.fotoDerecha, progreso);

            yield return null;
        }

        grupo.fotoIzquierda.localPosition = posOriginalIzq;
        FadeFoto(grupo.fotoIzquierda, 1f);

        grupo.fotoDerecha.localPosition = posOriginalDer;
        FadeFoto(grupo.fotoDerecha, 1f);
    }

    void FadeFoto(Transform foto, float alpha)
    {
        foreach (var r in foto.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                SetAlpha(mat, alpha);
            }
        }
    }
}