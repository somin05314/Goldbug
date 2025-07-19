using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CurveTool : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject controlPointPrefab;

    private Transform startPoint;
    private Transform endPoint;
    private Transform controlPoint;

    private bool isDragging = false;
    private LineRenderer line;

    public int curveResolution = 30; // 곡선 정밀도

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.useWorldSpace = true;
    }

    void Update()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && startPoint == null)
        {
            GameObject start = Instantiate(pointPrefab, mouseWorld, Quaternion.identity);
            startPoint = start.transform;
            isDragging = true;
        }

        if (isDragging && startPoint != null)
        {
            line.positionCount = 2;
            line.SetPosition(0, startPoint.position);
            line.SetPosition(1, mouseWorld);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            GameObject end = Instantiate(pointPrefab, mouseWorld, Quaternion.identity);
            endPoint = end.transform;

            Vector3 center = (startPoint.position + endPoint.position) / 2f;
            GameObject control = Instantiate(controlPointPrefab, center, Quaternion.identity);
            controlPoint = control.transform;

            isDragging = false;

            // 곡선 점 수 지정
            line.positionCount = curveResolution + 1;
        }

        // 곡선 그리기
        if (startPoint != null && endPoint != null && controlPoint != null && !isDragging)
        {
            DrawCurve();
        }
    }

    void DrawCurve()
    {
        for (int i = 0; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;

            Vector3 p0 = startPoint.position;
            Vector3 p1 = controlPoint.position;
            Vector3 p2 = endPoint.position;

            Vector3 pos = Mathf.Pow(1 - t, 2) * p0 +
                          2 * (1 - t) * t * p1 +
                          Mathf.Pow(t, 2) * p2;

            line.SetPosition(i, pos);
        }
    }
}
