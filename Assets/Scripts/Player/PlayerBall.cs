using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBall : MonoBehaviour, IResettable
{
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float originalAlpha;

    // 초기 상태 저장용
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            originalAlpha = sr.color.a;
    }

    private void Start()
    {
        // 초기 위치 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // BuildMode 이벤트 연결
        BuildModeManager.Instance.OnEnterPlacement += OnEnterPlacementMode;
        BuildModeManager.Instance.OnEnterSimulation += OnEnterSimulationMode;

        // 현재 모드에 맞춰 상태 적용
        if (BuildModeManager.Instance.IsPlacementMode())
            OnEnterPlacementMode();
        else
            OnEnterSimulationMode();
    }

    public void OnEnterPlacementMode()
    {
        ResetToInitialState(); // 초기 위치 복원도 포함됨

        rb.simulated = false;
        col.enabled = false;

        if (sr != null)
        {
            var c = sr.color;
            c.a = 0.3f;
            sr.color = c;
        }
    }

    public void OnEnterSimulationMode()
    {
        rb.simulated = true;
        col.enabled = true;

        if (sr != null)
        {
            var c = sr.color;
            c.a = originalAlpha;
            sr.color = c;
        }
    }

    public void ResetToInitialState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void OnDestroy()
    {
        if (BuildModeManager.Instance != null)
        {
            BuildModeManager.Instance.OnEnterPlacement -= OnEnterPlacementMode;
            BuildModeManager.Instance.OnEnterSimulation -= OnEnterSimulationMode;
        }
    }
}
