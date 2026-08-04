using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioInstrucciones : MonoBehaviour
{
    [Header("Referencias")]
    public Button botonInstrucciones;
    public HojaController hojaController;

    void Start()
    {
        botonInstrucciones.onClick.AddListener(ReproducirAudio);
    }

    public void ReproducirAudio()
    {
        if (hojaController != null && hojaController.audioVozEnOff != null)
        {
            hojaController.audioVozEnOff.Stop();
            hojaController.audioVozEnOff.Play();
        }
    }
}
