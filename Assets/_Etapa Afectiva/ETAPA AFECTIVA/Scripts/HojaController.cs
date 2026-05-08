using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HojaController : MonoBehaviour
{
    [Header("Referencias")]
    public MesaInteraction mesaInteraction;
    public AudioSource audioVozEnOff;
    public Button botonContinuar;
    public Button botonCerrar;
    public WordInputField[] campos;

    private Image imagenBotonContinuar;

    void Awake()
    {
        imagenBotonContinuar = botonContinuar.GetComponent<Image>();
    }

    void Start()
    {
        botonCerrar.onClick.AddListener(Cerrar);
        botonContinuar.onClick.AddListener(Continuar);
    }

    void OnEnable()
    {
        if (imagenBotonContinuar == null)
            imagenBotonContinuar = botonContinuar.GetComponent<Image>();

        botonContinuar.interactable = false;

        Color c = imagenBotonContinuar.color;
        c.a = 0.4f;
        imagenBotonContinuar.color = c;

        if (audioVozEnOff != null)
            audioVozEnOff.Play();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cerrar();

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                if (botonContinuar.interactable)
                    Continuar();
        }
    }

    public void ActualizarEstado()
    {
        bool todosCompletos = true;
        foreach (var campo in campos)
        {
            if (!campo.EstaCompleto())
            {
                todosCompletos = false;
                break;
            }
        }

        botonContinuar.interactable = todosCompletos;

        if (imagenBotonContinuar != null)
        {
            Color c = imagenBotonContinuar.color;
            c.a = todosCompletos ? 1f : 0.4f;
            imagenBotonContinuar.color = c;
        }
    }

    void Cerrar()
    {
        mesaInteraction.CerrarHoja();
    }

    void Continuar()
    {
        Debug.Log("¡Completado! Continuar a la siguiente parte.");
        mesaInteraction.CerrarHoja();
    }
}