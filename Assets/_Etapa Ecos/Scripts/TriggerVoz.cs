using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVoz : MonoBehaviour
{
    public AudioSource audioSource;
    private bool reproducido = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !reproducido)
        {
            reproducido = true;
            audioSource.Play();
        }
    }
}