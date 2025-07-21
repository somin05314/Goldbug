using UnityEngine;

public class CurveToolPlacer : MonoBehaviour, IPlaceableTool
{
    public GameObject curvePrefab;          // � ������ (CurveTool ����)
    public GameObject controlHandlePrefab;  // ������ �ڵ� ������ (�巡�׿�)

    public LineRenderer previewLine; // �ν����Ϳ� �Ҵ�

    private Vector2 dragStart, dragEnd;
    private CurveTool currentCurve;

    private enum CurveState { Idle, Dragging, Control }
    private CurveState state = CurveState.Idle;

    public void StartPlacing()
    {
        state = CurveState.Idle;
        currentCurve = null;
    }

    public void CancelPlacing()
    {
        if (currentCurve != null)
        {
            Destroy(currentCurve.gameObject);
            currentCurve = null;
        }
        state = CurveState.Idle;
    }

    public void UpdatePlacing()
    {
        if (state == CurveState.Dragging)
        {
            dragEnd = GetMouseWorldPosition();

            // ���� �̸�����
            if (previewLine != null)
            {
                previewLine.numCapVertices = 8; // 8~10 ������ �ڿ������� ����
                previewLine.enabled = true;
                previewLine.positionCount = 2;
                previewLine.SetPosition(0, dragStart);
                previewLine.SetPosition(1, dragEnd);
            }
        }
        else
        {
            if (previewLine != null)
                previewLine.enabled = false;
        }
    }


    public bool TryPlace()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return false;

        if (Input.GetMouseButtonDown(0) && state == CurveState.Idle)
        {
            dragStart = GetMouseWorldPosition();
            state = CurveState.Dragging;
        }

        if (Input.GetMouseButtonUp(0) && state == CurveState.Dragging)
        {
            dragEnd = GetMouseWorldPosition();

            GameObject curveObj = Instantiate(curvePrefab);
            var newCurve = curveObj.GetComponent<CurveTool>();

            //��ġ �Ϸ� �� previewCurve�� ���� �ʱ� (��, Placer�� ���� ����)
            if (newCurve != null)
            {
                newCurve.Initialize(dragStart, dragEnd, controlHandlePrefab);
            }

            state = CurveState.Control; // ���� �ܰ�� ��ȯ
            return false; // ���� ��ġ �� �ƴ�
        }

        if (state == CurveState.Control && Input.GetMouseButtonDown(0))
        {
            // ��Ʈ�� �ڵ� ���� �� �Ϸ��� ����
            return true; // ��ġ �Ϸ�� ����
        }

        return false;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
}
