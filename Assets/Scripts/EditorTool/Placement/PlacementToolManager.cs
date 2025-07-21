using UnityEngine;

public class PlacementToolManager : MonoBehaviour
{
    public static PlacementToolManager Instance { get; private set; }

    private GameObject currentToolObject;
    private IPlaceableTool currentTool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SelectPrefab(GameObject toolPrefab)
    {
        // 같은 도구 다시 클릭 → 해제
        if (currentToolObject != null && currentToolObject.name.Contains(toolPrefab.name))
        {
            DeselectTool();
            return;
        }

        DeselectTool(); // 기존 도구 제거

        currentToolObject = Instantiate(toolPrefab);
        currentTool = currentToolObject.GetComponent<IPlaceableTool>();

        if (currentTool != null)
        {
            currentTool.StartPlacing();
        }
        else
        {
            Debug.LogError("IPlaceableTool 인터페이스 없음!");
        }
    }

    private void Update()
    {
        if (currentTool == null) return;

        currentTool.UpdatePlacing();
        if (currentTool.TryPlace())
        {
            Debug.Log("설치 완료");
            DeselectTool();
        }
    }

    public void DeselectTool()
    {
        Debug.Log("삭제");
        if (currentTool != null)
        {
            currentTool.CancelPlacing();
        }

        if (currentToolObject != null)
        {
            Destroy(currentToolObject);
        }

        currentTool = null;
        currentToolObject = null;
    }
}
