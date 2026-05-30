using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairRotator : MonoBehaviour
{
    [Header("Rotaciones")]
    public Vector3 rotacionA = Vector3.zero;
    public Vector3 rotacionB = new Vector3(0f, 90f, 0f);

    [Header("Configuración")]
    public float velocidad = 1.5f;
    public string tagPlayer = "Player";

    private bool enPosicionA = true;
    private bool rotando = false;

    public void OnPlayerEnter(Collider other)
    {
        if (other.CompareTag(tagPlayer) && !rotando)
        {
            if (enPosicionA)
                StartCoroutine(Rotar(rotacionA, rotacionB));
            else
                StartCoroutine(Rotar(rotacionB, rotacionA));
        }
    }

    private IEnumerator Rotar(Vector3 desde, Vector3 hasta)
    {
        rotando = true;
        Quaternion inicio = Quaternion.Euler(desde);
        Quaternion fin = Quaternion.Euler(hasta);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidad;
            transform.localRotation = Quaternion.Slerp(inicio, fin, t);
            yield return null;
        }

        transform.localRotation = fin;
        enPosicionA = !enPosicionA;
        rotando = false;
    }
}