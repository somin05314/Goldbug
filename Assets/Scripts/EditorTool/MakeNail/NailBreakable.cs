using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class NailBreakable : MonoBehaviour
{
    public float breakForceThreshold = 15f; // �η����� �� ����
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

        // �� ��ġ���� ��ü������ ��� �ӵ� ����
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
        Debug.Log("�� �η���!");
        Destroy(joint); // Joint ����

        // ���Ѵٸ� �� ������Ʈ�� ����
        Destroy(gameObject);

        // ���⿡ ����Ʈ, �Ҹ� �߰� ����
    }
}
