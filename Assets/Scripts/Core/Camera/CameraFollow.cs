using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // ���� ��� (��: ����)
    public float smoothSpeed = 5f;   // �ε巯�� �̵� �ӵ�
    public Vector2 offset;           // 2D�� ������ (X, Y)

    void LateUpdate()
    {
        // ��ġ ����� ���� ������ ����
        if (BuildModeManager.Instance != null && BuildModeManager.Instance.IsPlacementMode())
            return;

        if (target == null) return;

        // ��ǥ ��ġ ��� (Z���� ����)
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z // Z�� �״�� ����
        );

        // �ε巴�� ����
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}

