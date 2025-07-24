using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerBall : MonoBehaviour, IModeSwitchHandler, IResettable
{
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float originalAlpha;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            originalAlpha = sr.color.a;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Start()
    {
        OnEnterPlacementMode();
    }

    public void OnEnterPlacementMode()
    {
        ResetToInitialState();

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
}
