using UnityEngine;

public class CurveToolPlacer : MonoBehaviour, IPlaceableTool
{
    public GameObject curvePrefab;          // 곡선 프리팹 (CurveTool 포함)
    public GameObject controlHandlePrefab;  // 제어점 핸들 프리팹 (드래그용)

    public LineRenderer previewLine; // 인스펙터에 할당

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

            // 직선 미리보기
            if (previewLine != null)
            {
                previewLine.numCapVertices = 8; // 8~10 정도면 자연스러운 원형
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

            //설치 완료 후 previewCurve에 넣지 않기 (즉, Placer가 관리 안함)
            if (newCurve != null)
            {
                newCurve.Initialize(dragStart, dragEnd, controlHandlePrefab);
            }

            state = CurveState.Control; // 다음 단계로 전환
            return false; // 아직 설치 끝 아님
        }

        if (state == CurveState.Control && Input.GetMouseButtonDown(0))
        {
            // 컨트롤 핸들 조정 후 완료라고 간주
            return true; // 설치 완료로 간주
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
