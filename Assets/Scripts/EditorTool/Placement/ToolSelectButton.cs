using UnityEngine;
using UnityEngine.UI;

public class ToolSelectButton : MonoBehaviour
{
    public GameObject toolPrefab; // BasicPlaceableTool ������

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PlacementToolManager.Instance.SelectPrefab(toolPrefab);
        });
    }
}

