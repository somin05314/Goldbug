using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D))]
public class CurveTool : MonoBehaviour
{
    private Vector2 startPoint, endPoint;
    private Transform controlPointTransform;

    private LineRenderer line;
    private PolygonCollider2D polygon;

    [SerializeField] private int segmentCount = 20;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        polygon = GetComponent<PolygonCollider2D>();
        line.numCapVertices = 8; // 8~10 정도면 자연스러운 원형
    }

    /// <summary>
    /// 곡선 초기화 (핸들 프리팹만)
    /// </summary>
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
            // dragger.onDragEnd도 제거 가능하지만, 유지해도 무방
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

        Vector2 control = controlPointTransform.position;

        Vector3[] centerPositions = new Vector3[segmentCount + 1];
        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            centerPositions[i] = CalculateBezierPoint(t, startPoint, control, endPoint);
        }

        // 곡선 라인의 굵이 기준 설정
        float halfWidth = line.widthMultiplier / 2f;

        List<Vector2> upperEdge = new List<Vector2>();
        List<Vector2> lowerEdge = new List<Vector2>();

        for (int i = 0; i < centerPositions.Length - 1; i++)
        {
            Vector2 p0 = centerPositions[i];
            Vector2 p1 = centerPositions[i + 1];
            Vector2 dir = (p1 - p0).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x); // 수직 벡터

            upperEdge.Add(p0 + normal * halfWidth);
            lowerEdge.Insert(0, p0 - normal * halfWidth); // 역순으로 삽입
        }

        // 마지막 점에 대해서도 추가
        {
            Vector2 last = centerPositions[^1];
            Vector2 beforeLast = centerPositions[^2];
            Vector2 dir = (last - beforeLast).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x);

            upperEdge.Add(last + normal * halfWidth);
            lowerEdge.Insert(0, last - normal * halfWidth);
        }

        List<Vector2> colliderShape = new List<Vector2>();
        colliderShape.AddRange(upperEdge);
        colliderShape.AddRange(lowerEdge);

        polygon.SetPath(0, colliderShape.ToArray());

        // LineRenderer 도 갱신
        line.positionCount = centerPositions.Length;
        line.SetPositions(centerPositions);
    }


    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0
             + 2 * (1 - t) * t * p1
             + Mathf.Pow(t, 2) * p2;
    }
}
