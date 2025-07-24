using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimulatableObject : MonoBehaviour, IModeSwitchHandler
{
    private Rigidbody2D rb;
    private float originalGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    private void Start()
    {
        if (BuildModeManager.Instance != null)
        {
            if (BuildModeManager.Instance.IsPlacementMode())
                OnEnterPlacementMode();
            else
                OnEnterSimulationMode();
        }
    }

    public void OnEnterPlacementMode()
    {
        // ���� ����� �����ϵ�, �߷¸� ����
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void OnEnterSimulationMode()
    {
        rb.gravityScale = originalGravity;
    }
}
