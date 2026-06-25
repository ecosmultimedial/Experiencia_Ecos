using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    [Header("Activar para resetear TODO al iniciar la escena")]
    [Tooltip("Marcá esto cuando quieras probar desde cero. Desmarcá para jugar normal.")]
    public bool resetearAlIniciar = false;

    void Start()
    {
        if (resetearAlIniciar)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("RESET COMPLETO: Todos los PlayerPrefs fueron borrados.");
        }
    }
}