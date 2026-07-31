using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockVoiceTrigger : MonoBehaviour
{
    public AudioSource voice;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (voice.isPlaying)
                voice.Stop();
            voice.Play();
        }
    }
}