using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonLink : MonoBehaviour
{
    public string url = "https://tu-url-aqui.com";

    public void AbrirLink()
    {
        Application.OpenURL(url);
    }
}