using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    private bool soundPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !soundPlayed)
        {
            audioSource.Play();
            soundPlayed = true;
        }
    }
}