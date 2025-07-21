using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementTool : MonoBehaviour
{
    public GameObject[] placeablePrefabs; // Inspector���� ��ġ ������ ������ ���
    private GameObject selectedPrefab;    // ���� ���õ� ������
    private Camera mainCamera;
    private GameObject ghostInstance;

    void Start()
    {
        mainCamera = Camera.main;

        // �⺻ ���� ������ (0��°)
        if (placeablePrefabs.Length > 0)
        {
            selectedPrefab = placeablePrefabs[0];
        }
    }

    void Update()
    {
        if (!BuildModeManager.Instance.IsPlacementMode())
            return;

        // UI ���� Ŭ���� ��� ��ġ���� ����
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (ghostInstance != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            ghostInstance.transform.position = mousePos;
        }

        // Physics2D.Raycast�� ��ü ������ Ȯ��
        if (Input.GetMouseButtonDown(0) && selectedPrefab != null && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // ������Ʈ ���� Ŭ���ߴ��� üũ
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                // ���� ���� ���� �� ��ġ �� ��
                return;
            }

            // �� ���� �� ��ġ
            Instantiate(selectedPrefab, mousePos, Quaternion.identity);
        }
    }

    // ���߿� UI���� ȣ���� �� �ִ� ������ ���� �Լ�
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

        // ��Ʈ ��Ÿ�� ����
        foreach (var sr in ghostInstance.GetComponentsInChildren<SpriteRenderer>())
        {
            var color = sr.color;
            color.a = 0.5f; // ���� ����
            sr.color = color;
        }

        // �浹 ��Ȱ��ȭ
        foreach (var col in ghostInstance.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }
}