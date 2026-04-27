using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceFieldTrigger : MonoBehaviour
{
    public Question3DManager questionManager;

    public Image flashImage;        // ← UI blanca en pantalla
    public float flashSpeed = 10f;   // velocidad del flash

    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(Sequence());
        }
    }

    IEnumerator Sequence()
    {
        yield return StartCoroutine(FlashEffect());

        questionManager.OpenQuestions();
    }

    IEnumerator FlashEffect()
    {
        // APARECE (fade in)
        while (flashImage.color.a < 2f)
        {
            Color c = flashImage.color;
            c.a += Time.deltaTime * flashSpeed;
            flashImage.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        // DESAPARECE (fade out)
        while (flashImage.color.a > 0f)
        {
            Color c = flashImage.color;
            c.a -= Time.deltaTime * flashSpeed;
            flashImage.color = c;
            yield return null;
        }
    }
}