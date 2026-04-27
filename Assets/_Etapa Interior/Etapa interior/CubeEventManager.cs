using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventManager : MonoBehaviour
{
    public GameObject continueCanvas;

    public List<Renderer> cubes;

    public GameObject portal;

    public float fadeDuration = 3f;

    private bool activated = false;
    private bool canvasVisible = false;

    public void StartEvent()
    {
        if (!activated)
        {
            activated = true;
            StartCoroutine(ShowCanvasAfterDelay());
        }
    }

    IEnumerator ShowCanvasAfterDelay()
    {
        yield return new WaitForSeconds(15f);

        continueCanvas.SetActive(true);
        canvasVisible = true;
    }

    void Update()
    {
        if (canvasVisible && Input.GetKeyDown(KeyCode.Return))
        {
            ContinueExperience();
        }
    }

    public void ContinueExperience()
    {
        canvasVisible = false;

        continueCanvas.SetActive(false);

        StartCoroutine(FadeCubes());
    }

    IEnumerator FadeCubes()
    {
        float time = 0;

        List<Material> materials = new List<Material>();

        foreach (Renderer r in cubes)
        {
            materials.Add(r.material);
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(1, 0, time / fadeDuration);

            foreach (Material m in materials)
            {
                Color color = m.color;
                color.a = alpha;
                m.color = color;
            }

            yield return null;
        }

        foreach (Renderer r in cubes)
        {
            r.gameObject.SetActive(false);
        }

        // Activar portal
        if (portal != null)
        {
            portal.SetActive(true);
        }
    }
}