using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelBloqueoUI : MonoBehaviour
{
    public void Cerrar()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}