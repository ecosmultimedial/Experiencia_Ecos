using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockVoiceTrigger : MonoBehaviour
{
    public AudioSource voice;

    private bool played = false;

    void OnTriggerEnter(Collider other)
    {
        if (!played && other.CompareTag("Player"))
        {
            played = true;
            voice.Play();
        }
    }
}