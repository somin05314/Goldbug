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
        // ���� ���� �ٽ� Ŭ�� �� ����
        if (currentToolObject != null && currentToolObject.name.Contains(toolPrefab.name))
        {
            DeselectTool();
            return;
        }

        DeselectTool(); // ���� ���� ����

        currentToolObject = Instantiate(toolPrefab);
        currentTool = currentToolObject.GetComponent<IPlaceableTool>();

        if (currentTool != null)
        {
            currentTool.StartPlacing();
        }
        else
        {
            Debug.LogError("IPlaceableTool �������̽� ����!");
        }
    }

    private void Update()
    {
        if (currentTool == null) return;

        currentTool.UpdatePlacing();
        if (currentTool.TryPlace())
        {
            Debug.Log("��ġ �Ϸ�");
            DeselectTool();
        }
    }

    public void DeselectTool()
    {
        Debug.Log("����");
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
