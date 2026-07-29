using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [Header("Pincel")]
    public Color colorActual = Color.black;
    public int brushSize = 20;

    [Header("Pared")]
    public Renderer wallRenderer;  // El renderer de tuneldibujo (1)

    private Texture2D paintTexture;
    private bool initialized = false;

    void OnEnable()
    {
        InicializarTextura();
    }

    void InicializarTextura()
    {
        if (initialized) return;

        // Crear textura nueva sobre la que vamos a pintar
        Texture originalTex = wallRenderer.material.mainTexture;
        if (originalTex != null)
        {
            paintTexture = new Texture2D(
                originalTex.width,
                originalTex.height,
                TextureFormat.RGBA32,
                false
            );
            // Copiar textura original como base
            RenderTexture rt = RenderTexture.GetTemporary(
                originalTex.width, originalTex.height
            );
            Graphics.Blit(originalTex, rt);
            RenderTexture.active = rt;
            paintTexture.ReadPixels(
                new Rect(0, 0, rt.width, rt.height), 0, 0
            );
            paintTexture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }
        else
        {
            // Si no hay textura, crear una blanca
            paintTexture = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[2048 * 2048];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = Color.white;
            paintTexture.SetPixels(pixels);
            paintTexture.Apply();
        }

        wallRenderer.material.mainTexture = paintTexture;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        // No pintar si el mouse está sobre un elemento de UI
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(0))
            Pintar();
    }

    void Pintar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Verificar que pegó en la pared correcta
            if (hit.collider.gameObject == wallRenderer.gameObject ||
                hit.collider.transform.IsChildOf(wallRenderer.transform))
            {
                Vector2 uv = hit.textureCoord;
                int x = (int)(uv.x * paintTexture.width);
                int y = (int)(uv.y * paintTexture.height);
                PintarPincel(x, y);
            }
        }
    }

    void PintarPincel(int centerX, int centerY)
    {
        for (int x = -brushSize; x <= brushSize; x++)
        {
            for (int y = -brushSize; y <= brushSize; y++)
            {
                if (x * x + y * y <= brushSize * brushSize)
                {
                    int px = Mathf.Clamp(centerX + x, 0, paintTexture.width - 1);
                    int py = Mathf.Clamp(centerY + y, 0, paintTexture.height - 1);
                    paintTexture.SetPixel(px, py, colorActual);
                }
            }
        }
        paintTexture.Apply();
    }

    public void CambiarColor(Color nuevoColor)
    {
        colorActual = nuevoColor;
    }
}