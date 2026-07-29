using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Animator pivotIzq;
    public Animator pivotDer;
    public AudioSource doorSound;

    private bool opened = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            opened = true;
            pivotIzq.SetTrigger("Abrir");
            pivotDer.SetTrigger("Abrir");

            if (doorSound != null)
                doorSound.Play();
        }
    }
}