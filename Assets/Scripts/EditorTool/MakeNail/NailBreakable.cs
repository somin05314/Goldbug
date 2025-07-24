using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class NailBreakable : MonoBehaviour
{
    public float breakForceThreshold = 15f; // 부러지는 힘 기준
    public float breakTorqueThreshold = 10f;

    private HingeJoint2D joint;
    private Rigidbody2D connectedBody;

    void Start()
    {
        joint = GetComponent<HingeJoint2D>();
        connectedBody = joint.connectedBody;
    }

    void FixedUpdate()
    {
        if (joint == null || connectedBody == null)
            return;

        // 못 위치에서 물체까지의 상대 속도 측정
        Vector2 relativeVelocity = connectedBody.GetPointVelocity(joint.connectedAnchor);
        float forceMagnitude = relativeVelocity.magnitude * connectedBody.mass;

        float torque = Mathf.Abs(connectedBody.angularVelocity * connectedBody.inertia);

        if (forceMagnitude > breakForceThreshold || torque > breakTorqueThreshold)
        {
            BreakNail();
        }
    }

    private void BreakNail()
    {
        Debug.Log("못 부러짐!");
        Destroy(joint); // Joint 제거

        // 원한다면 못 오브젝트도 제거
        Destroy(gameObject);

        // 여기에 이펙트, 소리 추가 가능
    }
}
