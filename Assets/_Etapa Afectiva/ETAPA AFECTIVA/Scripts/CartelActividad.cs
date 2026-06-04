using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartelActividad : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button botonContinuar;

    [Header("Sistema de luces guía")]
    [SerializeField] private SistemaLucesGuia sistemaLuces;

    private Image imagenBoton;
    private bool actividadCompletada = false;

    private void Awake()
    {
        if (botonContinuar != null)
            imagenBoton = botonContinuar.GetComponent<Image>();
    }

    private void Start()
    {
        if (botonContinuar != null)
        {
            botonContinuar.onClick.AddListener(CompletarActividad);
            OcultarBoton();
        }
    }

    private void Update()
    {
        if (!actividadCompletada && botonContinuar != null && botonContinuar.interactable)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CompletarActividad();
            }
        }
    }

    public void MostrarBoton()
    {
        if (actividadCompletada) return;
        botonContinuar.interactable = true;
        SetAlpha(1f);

        // Liberar el cursor para que el jugador pueda clickear
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OcultarBoton()
    {
        if (actividadCompletada) return;
        botonContinuar.interactable = false;
        SetAlpha(0f);

        // Volver a bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetAlpha(float a)
    {
        if (imagenBoton == null) return;
        Color c = imagenBoton.color;
        c.a = a;
        imagenBoton.color = c;
    }

    private void CompletarActividad()
    {
        if (actividadCompletada) return;

        actividadCompletada = true;
        botonContinuar.interactable = false;
        SetAlpha(0f);

        // Volver a bloquear el cursor para seguir jugando
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (sistemaLuces != null)
            sistemaLuces.IniciarGrupo3();

        Debug.Log("Actividad del cartel completada. Tercer grupo de luces activado.");
    }

    public bool EstaCompletada => actividadCompletada;
}