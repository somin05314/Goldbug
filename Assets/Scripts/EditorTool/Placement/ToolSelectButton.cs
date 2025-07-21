using UnityEngine;
using UnityEngine.UI;

public class ToolSelectButton : MonoBehaviour
{
    public GameObject toolPrefab; // BasicPlaceableTool ÇÁ¸®ÆÕ

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PlacementToolManager.Instance.SelectPrefab(toolPrefab);
        });
    }
}

