using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionLenta : MonoBehaviour
{
    [SerializeField] private Vector3 ejeRotacion = Vector3.up;
    [SerializeField] private float velocidadRotacion = 10f;

    [Header("Flotado opcional (sube y baja)")]
    [SerializeField] private bool flotar = false;
    [SerializeField] private float amplitudFlotado = 0.1f;
    [SerializeField] private float velocidadFlotado = 1f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(ejeRotacion * velocidadRotacion * Time.deltaTime);

        if (flotar)
        {
            float nuevoY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlotado) * amplitudFlotado;
            transform.localPosition = new Vector3(posicionInicial.x, nuevoY, posicionInicial.z);
        }
    }
}