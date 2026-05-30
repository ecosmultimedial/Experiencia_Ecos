using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerZone : MonoBehaviour
{
    public AnimationController animationController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animationController.ActivarAnimacion();
        }
    }
}