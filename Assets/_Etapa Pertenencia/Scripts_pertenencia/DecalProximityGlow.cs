using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalProximityGlow : MonoBehaviour
{
    public Transform player;
    public float activationRange = 4f;
    public float fadeSpeed = 2f;
    public float flickerSpeed = 8f;
    public float flickerAmount = 0.3f;

    private DecalProjector decalProjector;
    private float targetOpacity = 0f;
    private float currentOpacity = 0f;

    void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
        decalProjector.fadeFactor = 0f;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= activationRange)
        {
            // Efecto titilar cuando está cerca
            float flicker = 1f + Mathf.Sin(Time.time * flickerSpeed) * flickerAmount;
            targetOpacity = Mathf.Clamp01(flicker);
        }
        else
        {
            targetOpacity = 0f;
        }

        currentOpacity = Mathf.Lerp(currentOpacity, targetOpacity, Time.deltaTime * fadeSpeed);
        decalProjector.fadeFactor = currentOpacity;
    }
}