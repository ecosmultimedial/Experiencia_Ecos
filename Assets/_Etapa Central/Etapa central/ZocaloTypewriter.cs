using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ZocaloTypewriter : MonoBehaviour
{
    [Header("Configuracion")]
    [TextArea] public string texto;
    public TMP_Text campoTexto;
    public float velocidad = 0.05f;
    [Header("Auto-ocultar")]
    [Tooltip("0 = no desaparece solo.")]
    public float tiempoParaOcultar = 0f;
    private GameObject canvasPadre;
    private void Awake()
    {
        canvasPadre = transform.root.gameObject;
    }
    public void Activar()
    {
        canvasPadre.SetActive(true);
        campoTexto.text = "";
        StartCoroutine(EscribirTexto());
    }
    public void Desactivar()
    {
        StopAllCoroutines();
        canvasPadre.SetActive(false);
    }
    public void DetenerEscritura()
    {
        StopAllCoroutines();
    }
    public void LimpiarTexto()
    {
        if (campoTexto != null) campoTexto.text = "";
    }
    private IEnumerator EscribirTexto()
    {
        foreach (char letra in texto)
        {
            campoTexto.text += letra;
            yield return new WaitForSeconds(velocidad);
        }
        if (tiempoParaOcultar > 0)
        {
            yield return new WaitForSeconds(tiempoParaOcultar);
            canvasPadre.SetActive(false);
        }
    }
}