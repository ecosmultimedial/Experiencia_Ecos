using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTriggerZone : MonoBehaviour
{
    public StairRotator stairRotator; // arrastrás la escalera acá en el Inspector

    private void OnTriggerEnter(Collider other)
    {
        stairRotator.OnPlayerEnter(other);
    }
}