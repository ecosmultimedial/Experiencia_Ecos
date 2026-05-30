using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Referencias")]
    public PopupController popupController;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    public void ActivarAnimacion()
    {
        animator.speed = 1f;
        Invoke("MostrarPopup", 2f);
    }

    private void MostrarPopup()
    {
        popupController.MostrarPopup();
    }
}