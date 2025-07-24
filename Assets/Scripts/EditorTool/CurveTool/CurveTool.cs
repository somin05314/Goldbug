using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D))]
public class CurveTool : MonoBehaviour
{
    private Vector2 startPoint, endPoint;
    private Transform controlPointTransform;

    private LineRenderer line;
    private PolygonCollider2D polygon;
    private CurveToolNailPlacer nailPlacer;

    [SerializeField] private int segmentCount = 20;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        polygon = GetComponent<PolygonCollider2D>();
        line.useWorldSpace = false;
        line.numCapVertices = 8;
        nailPlacer = GetComponent<CurveToolNailPlacer>();
    }

    public void Initialize(Vector2 start, Vector2 end, GameObject controlHandlePrefab)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        startPoint = start;
        endPoint = end;

        Vector2 mid = (start + end) / 2f;
        GameObject handle = Instantiate(controlHandlePrefab, mid, Quaternion.identity);
        controlPointTransform = handle.transform;

        var dragger = handle.GetComponent<ControlPointDragger>();
        if (dragger != null)
        {
            dragger.onPositionChanged = (pos) => UpdateCurve();
            dragger.onDragEnd = () =>
            {
                UpdateCurve();
                nailPlacer.UpdateNailSlots();
            };
        }

        UpdateCurve();
    }

    public void UpdatePoints(Vector2 start, Vector2 end)
    {
        startPoint = start;
        endPoint = end;
        UpdateCurve();
    }

    public void UpdateCurve()
    {
        if (controlPointTransform == null || line == null || polygon == null)
            return;

        Vector3[] centerPositions = CalculateCenterPoints();
        UpdateLineRenderer(centerPositions);
        List<Vector2> colliderShape = BuildColliderShape(centerPositions);
        polygon.SetPath(0, colliderShape.ToArray());
    }

    private Vector3[] CalculateCenterPoints()
    {
        Vector2 control = controlPointTransform.position;
        Vector3[] centerPositions = new Vector3[segmentCount + 1];

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            centerPositions[i] = CalculateBezierPoint(t, startPoint, control, endPoint);
        }

        return centerPositions;
    }

    private void UpdateLineRenderer(Vector3[] centerPositions)
    {
        line.positionCount = centerPositions.Length;
        line.SetPositions(centerPositions);
    }

    private List<Vector2> BuildColliderShape(Vector3[] centerPositions)
    {
        float halfWidth = line.widthMultiplier / 2f;
        int capSegments = 8;

        List<Vector2> upperEdge = new();
        List<Vector2> lowerEdge = new();
        Vector2[] center2D = System.Array.ConvertAll(centerPositions, p => (Vector2)p);

        for (int i = 0; i < center2D.Length - 1; i++)
        {
            Vector2 dir = (center2D[i + 1] - center2D[i]).normalized;
            Vector2 normal = new(-dir.y, dir.x);
            upperEdge.Add(center2D[i] + normal * halfWidth);
            lowerEdge.Insert(0, center2D[i] - normal * halfWidth);
        }

        Vector2 endDir = (center2D[^1] - center2D[^2]).normalized;
        Vector2 endNormal = new(-endDir.y, endDir.x);
        upperEdge.Add(center2D[^1] + endNormal * halfWidth);
        lowerEdge.Insert(0, center2D[^1] - endNormal * halfWidth);

        AddRoundCap(lowerEdge, center2D[0], center2D[1], halfWidth, capSegments, false);
        AddRoundCap(upperEdge, center2D[^1], center2D[^2], halfWidth, capSegments, false); // 오른쪽도 false로 수정

        List<Vector2> shape = new();
        shape.AddRange(upperEdge);
        shape.AddRange(lowerEdge);
        return shape;
    }

    private void AddRoundCap(List<Vector2> edgeList, Vector2 center, Vector2 directionPoint, float radius, int segments, bool clockwise)
    {
        Vector2 dir = (center - directionPoint).normalized;
        Vector2 baseNormal = new(-dir.y, dir.x);

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.PI * (i / (float)segments);
            float signedAngle = clockwise ? -angle : angle;
            Vector2 offset = Quaternion.Euler(0, 0, Mathf.Rad2Deg * signedAngle) * (-baseNormal * radius);
            edgeList.Add(center + offset);
        }
    }

    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0
             + 2 * (1 - t) * t * p1
             + Mathf.Pow(t, 2) * p2;
    }

    public Vector3[] GetCenterPoints()
    {
        return CalculateCenterPoints();
    }
}
