using UnityEngine;
using UnityEngine.UI;

public class ToolSelectButton : MonoBehaviour
{
    public GameObject prefabToPlace; // 이 버튼이 선택할 프리팹



    void Start()
    {
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button 컴포넌트가 없습니다: " + gameObject.name);
            return;
        }

        PlacementTool placementTool = FindObjectOfType<PlacementTool>();
        if (placementTool == null)
        {
            Debug.LogError("PlacementTool이 씬에 없습니다!");
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
