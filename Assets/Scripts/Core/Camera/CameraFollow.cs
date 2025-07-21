using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // 따라갈 대상 (예: 구슬)
    public float smoothSpeed = 5f;   // 부드러운 이동 속도
    public Vector2 offset;           // 2D용 오프셋 (X, Y)

    void LateUpdate()
    {
        // 배치 모드일 때는 따라가지 않음
        if (BuildModeManager.Instance != null && BuildModeManager.Instance.IsPlacementMode())
            return;

        if (target == null) return;

        // 목표 위치 계산 (Z축은 고정)
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z // Z는 그대로 유지
        );

        // 부드럽게 따라감
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}

