using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorLightController : MonoBehaviour
{
    public Transform player;
    public Light[] corridorLights;
    public float activationDistance = 4f;

    void Update()
    {
        foreach (Light light in corridorLights)
        {
            float distance = Vector3.Distance(player.position, light.transform.position);

            if (distance <= activationDistance)
            {
                light.enabled = true;
            }
            else
            {
                light.enabled = false;
            }
        }
    }
}