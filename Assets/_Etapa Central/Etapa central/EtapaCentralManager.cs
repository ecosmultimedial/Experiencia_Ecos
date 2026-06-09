using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtapaCentralManager : MonoBehaviour
{
    [Header("Primera visita")]
    public GameObject canvasBienvenida;
    public AudioSource vozEnOff;

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

    private void Start()
    {
        // 1. Posicionar al player segun de donde viene
        PosicionarPlayerSegunOrigen();

        // 2. Desplegar el puente correspondiente
        DesplegarPuenteSegunOrigen();

        // 3. Logica de bienvenida (sin cambios)
        canvasBienvenida.SetActive(false);
        if (PlayerPrefs.GetInt("VozCentralReproducida", 0) == 0)
        {
            MostrarBienvenida();
            PlayerPrefs.SetInt("VozCentralReproducida", 1);
            PlayerPrefs.Save();
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
                if (puenteInterior != null) puenteInterior.ExtenderPuente();
                break;
        }
    }

    private void ExtenderInstant(PuenteExtensible puente)
    {
        if (puente == null) return;
        puente.transform.localPosition = puente.posicionExtendida;
    }

    private void MostrarBienvenida()
    {
        canvasBienvenida.SetActive(true);
        vozEnOff.Play();
        Invoke(nameof(OcultarBienvenida), vozEnOff.clip.length);
    }

    private void OcultarBienvenida()
    {
        canvasBienvenida.SetActive(false);
    }
}