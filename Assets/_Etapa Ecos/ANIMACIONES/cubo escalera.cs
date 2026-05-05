using UnityEngine;

public class CuboActiva : MonoBehaviour
{
    public Transform escalera;

    public Vector3 rotacionInicial;
    public Vector3 rotacionFinal;
    public float velocidad = 2f;

    private bool activar = false;

    void Start()
    {
        // Arranca en la rotación inicial
        escalera.rotation = Quaternion.Euler(rotacionInicial);
    }

    void Update()
    {
        if (activar)
        {
            escalera.rotation = Quaternion.Lerp(
                escalera.rotation,
                Quaternion.Euler(rotacionFinal),
                velocidad * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activar = true;
        }
    }
}