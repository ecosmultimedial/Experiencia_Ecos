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
    public WordInputField[] campos;

    private Image imagenBotonContinuar;

    void Start()
    {
        botonContinuar.onClick.AddListener(Continuar);
        imagenBotonContinuar = botonContinuar.GetComponent<Image>();
    }

    void OnEnable()
    {
        botonContinuar.interactable = false;

        // Solo bajar opacidad, no cambiar color
        if (imagenBotonContinuar != null)
        {
            Color c = imagenBotonContinuar.color;
            c.a = 0.4f; // semitransparente = desactivado
            imagenBotonContinuar.color = c;
        }

        if (audioVozEnOff != null)
            audioVozEnOff.Play();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            // Enter solo funciona si el botón está activo
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

        // Solo cambiar opacidad
        if (imagenBotonContinuar != null)
        {
            Color c = imagenBotonContinuar.color;
            c.a = todosCompletos ? 1f : 0.4f;
            imagenBotonContinuar.color = c;
        }
    }

    void Continuar()
    {
        Debug.Log("¡Completado! Continuar a la siguiente parte.");
        mesaInteraction.CerrarHoja();
    }
}