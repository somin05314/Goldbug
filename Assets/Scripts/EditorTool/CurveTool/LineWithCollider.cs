using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class LineWithCollider : MonoBehaviour
{
    private LineRenderer line;
    private EdgeCollider2D edgeCollider;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        SyncColliderWithLine();
    }

    void SyncColliderWithLine()
    {
        int pointCount = line.positionCount;
        Vector2[] colliderPoints = new Vector2[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            Vector3 worldPos = line.GetPosition(i);
            colliderPoints[i] = new Vector2(worldPos.x, worldPos.y); // 2D·Î º¯È¯
        }

        edgeCollider.points = colliderPoints;
    }
}
