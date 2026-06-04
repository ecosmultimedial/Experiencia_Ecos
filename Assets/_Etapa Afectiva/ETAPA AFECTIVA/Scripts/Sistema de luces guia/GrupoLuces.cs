using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrupoLuces : MonoBehaviour
{
    [Tooltip("Delay entre el encendido de una luz y la siguiente")]
    public float delayEntreLuces = 0.5f;

    public void IniciarSecuencia()
    {
        StartCoroutine(SecuenciaFadeIn());
    }

    private IEnumerator SecuenciaFadeIn()
    {
        // Recorre las luces hijas en el orden en que están en la jerarquía
        foreach (Transform hija in transform)
        {
            LuzGuia luz = hija.GetComponent<LuzGuia>();
            if (luz != null)
            {
                StartCoroutine(luz.FadeIn());
                yield return new WaitForSeconds(delayEntreLuces);
            }
        }
    }
}