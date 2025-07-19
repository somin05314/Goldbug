using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ControlPointDragger : MonoBehaviour
{
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
        offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z) + offset;
            transform.position = newPos;
        }
    }
}
