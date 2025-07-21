using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementTool : MonoBehaviour
{
    public GameObject[] placeablePrefabs; // Inspector에서 배치 가능한 프리팹 등록
    private GameObject selectedPrefab;    // 현재 선택된 프리팹
    private Camera mainCamera;
    private GameObject ghostInstance;

    void Start()
    {
        mainCamera = Camera.main;

        // 기본 선택 프리팹 (0번째)
        if (placeablePrefabs.Length > 0)
        {
            selectedPrefab = placeablePrefabs[0];
        }
    }

    void Update()
    {
        if (!BuildModeManager.Instance.IsPlacementMode())
            return;

        // UI 위를 클릭한 경우 배치하지 않음
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (ghostInstance != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            ghostInstance.transform.position = mousePos;
        }

        // Physics2D.Raycast로 물체 위인지 확인
        if (Input.GetMouseButtonDown(0) && selectedPrefab != null && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 오브젝트 위에 클릭했는지 체크
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                // 뭔가 위에 있음 → 배치 안 함
                return;
            }

            // 빈 공간 → 설치
            Instantiate(selectedPrefab, mousePos, Quaternion.identity);
        }
    }

    // 나중에 UI에서 호출할 수 있는 프리팹 변경 함수
    public void SelectPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
        CreateGhostPreview(prefab);
    }

    void CreateGhostPreview(GameObject prefab)
    {
        if (ghostInstance != null)
            Destroy(ghostInstance);

        ghostInstance = Instantiate(prefab);
        ghostInstance.name = "GhostPreview";

        // 고스트 스타일 설정
        foreach (var sr in ghostInstance.GetComponentsInChildren<SpriteRenderer>())
        {
            var color = sr.color;
            color.a = 0.5f; // 투명도 조절
            sr.color = color;
        }

        // 충돌 비활성화
        foreach (var col in ghostInstance.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }
}