using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBall : MonoBehaviour, IResettable
{
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float originalAlpha;

    // �ʱ� ���� �����
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
        // �ʱ� ��ġ ����
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // BuildMode �̺�Ʈ ����
        BuildModeManager.Instance.OnEnterPlacement += OnEnterPlacementMode;
        BuildModeManager.Instance.OnEnterSimulation += OnEnterSimulationMode;

        // ���� ��忡 ���� ���� ����
        if (BuildModeManager.Instance.IsPlacementMode())
            OnEnterPlacementMode();
        else
            OnEnterSimulationMode();
    }

    public void OnEnterPlacementMode()
    {
        ResetToInitialState(); // �ʱ� ��ġ ������ ���Ե�

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
