using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class AlembicAutoPlay : MonoBehaviour
{
    public AlembicStreamPlayer alembic;
    public float speed = 1f;
    public float duration = 1.5f;

    void Update()
    {
        if (alembic == null) return;

        alembic.CurrentTime += Time.deltaTime * speed;

        if (alembic.CurrentTime > duration)
        {
            alembic.CurrentTime = 0f;
        }
    }
}