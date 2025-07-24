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
        // 물리 계산은 유지하되, 중력만 꺼줌
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void OnEnterSimulationMode()
    {
        rb.gravityScale = originalGravity;
    }
}
