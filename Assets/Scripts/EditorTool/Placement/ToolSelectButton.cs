using UnityEngine;
using UnityEngine.UI;

public class ToolSelectButton : MonoBehaviour
{
    public GameObject prefabToPlace; // �� ��ư�� ������ ������



    void Start()
    {
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button ������Ʈ�� �����ϴ�: " + gameObject.name);
            return;
        }

        PlacementTool placementTool = FindObjectOfType<PlacementTool>();
        if (placementTool == null)
        {
            Debug.LogError("PlacementTool�� ���� �����ϴ�!");
            return;
        }

        button.onClick.AddListener(() =>
        {
            placementTool.SelectPrefab(prefabToPlace);
        });

        GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<PlacementTool>().SelectPrefab(prefabToPlace);
        });
    }
}
