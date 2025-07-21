using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicPlaceableTool : MonoBehaviour, IPlaceableTool
{
    public GameObject ghostPrefab;
    private GameObject currentGhost;
    public GameObject prefabToPlace;

    public void StartPlacing()
    {
        currentGhost = Instantiate(ghostPrefab);
    }

    public void CancelPlacing()
    {
        if(currentGhost != null) Destroy(currentGhost);
    }

    public void UpdatePlacing()
    {
        if (currentGhost != null)
        {
            Vector2 pos = GetMouseWorldPos();
            currentGhost.transform.position = pos;
        }
    }

    public bool TryPlace()
    {
        Debug.Log("ABC");
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 placePos = GetMouseWorldPos();
            Instantiate(prefabToPlace, placePos, Quaternion.identity);
            return true;
        }
        return false;

    }

    private Vector2 GetMouseWorldPos()
    {
        Debug.Log("마우스 입력");
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 0f; // 2D일 경우 보통 Z는 0
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
