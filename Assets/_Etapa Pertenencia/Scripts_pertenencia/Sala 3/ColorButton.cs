using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public Color color;
    public WallPainter wallPainter;

    public void OnClick()
    {
        wallPainter.CambiarColor(color);
    }
}
