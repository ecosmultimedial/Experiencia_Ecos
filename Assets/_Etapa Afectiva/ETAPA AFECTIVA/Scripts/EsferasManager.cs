using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsferasManager : MonoBehaviour
{
    public static EsferasManager Instance;

    [Header("Referencias")]
    public GameObject portal;
    public int totalEsferas = 4;
    public float demora = 2f;

    private int esferasVistas = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        portal.SetActive(false);
    }

    public void EsferaVista()
    {
        esferasVistas++;
        Debug.Log($"Esferas vistas: {esferasVistas}/{totalEsferas}");

        if (esferasVistas >= totalEsferas)
            StartCoroutine(ActivarPortalConDemora());
    }

    IEnumerator ActivarPortalConDemora()
    {
        yield return new WaitForSeconds(demora);
        portal.SetActive(true);
        Debug.Log("¡Portal activado!");
    }
}