using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class ControlPointDragger : MonoBehaviour
{
    public Action<Vector2> onPositionChanged;
    public Action onDragEnd;

    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f; // z값 고정
        offset = transform.position - mouseWorld;
    }

    void OnMouseUp()
    {
        isDragging = false;
        onDragEnd?.Invoke();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f; // z값 고정

            Vector3 newPos = mouseWorld + offset;
            newPos.z = 0f; // 안전하게 z를 다시 0으로

            transform.position = newPos;

            onPositionChanged?.Invoke(new Vector2(newPos.x, newPos.y)); // z 제거
        }
    }
}
