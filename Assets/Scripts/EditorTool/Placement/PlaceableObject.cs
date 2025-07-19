using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlaceableObject : MonoBehaviour
{
    private bool isSelected = false;
    private bool isDragging = false;
    private Vector3 offset;

    void OnMouseDown()
    {
        isDragging = true;
        isSelected = true; 

        // ���콺�� ������Ʈ ���� �Ÿ� ���
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (!BuildModeManager.Instance.IsPlacementMode())
            return;

        if (isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
            transform.position = newPos;
        }

        // R Ű ȸ��
        if (isSelected && Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0, 0, 15f);
        }

        // Delete Ű ����
        if (isSelected && Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(gameObject);
        }

        // ��Ŭ�� ���� ����
        if (isSelected && Input.GetMouseButtonDown(1))
        {
            isSelected = false;
        }
    }

}

