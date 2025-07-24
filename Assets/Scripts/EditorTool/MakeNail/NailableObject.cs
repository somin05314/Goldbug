using System.Collections.Generic;
using UnityEngine;

public class NailableObject : MonoBehaviour
{
    public GameObject nailSlotPrefab;      // ���� ǥ�� ������
    public Rigidbody2D targetBody;         // ���� ������ ���
    private List<GameObject> registeredSlots = new List<GameObject>();

    // �ܺο��� ���� ��ġ�� �Ѱ� �޾� ���� ����
    public void CreateNailSlotAt(Vector2 localPos)
    {
        Vector2 worldPos = transform.TransformPoint(localPos);

        GameObject slot = Instantiate(nailSlotPrefab, worldPos, Quaternion.identity, transform);
        var nailSlot = slot.GetComponent<NailSlot>();
        if (nailSlot != null)
        {
            nailSlot.Initialize(this, transform); // �� ��ġ �� �θ� �������� ó��
        }
    }

    // ���Կ��� ȣ��Ǵ� �� ��ġ �Լ�
    public void NailAt(Transform slotTransform)
    {
        // �� ������ ����
        GameObject nailObj = Instantiate(NailManager.Instance.nailPrefab, slotTransform.position, Quaternion.identity);

        // HingeJoint2D �߰�
        var joint = nailObj.AddComponent<HingeJoint2D>();
        joint.connectedBody = targetBody;

        // �� ��ġ�� �������� ȸ���ϰ� ����
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero; // ���� �߾�
        joint.connectedAnchor = targetBody.transform.InverseTransformPoint(slotTransform.position);

        joint.enableCollision = false; // ���� ��ü ���� �浹�� ��Ȱ��ȭ

        // ȸ���� ������ ����� ���� ���� ����
        joint.useMotor = true;
        JointMotor2D motor = new JointMotor2D
        {
            motorSpeed = 0f, // �߸� �ӵ�. (ȸ���� ������ ��Ű�� ����)
            maxMotorTorque = 10f // ȸ���� ���� ����. �������� �� ��鸲, �� õõ�� ������
        };
        joint.motor = motor;

        // �� ���� ����
        NailManager.Instance.UseNail();

        // ���� ����
        Destroy(slotTransform.gameObject);
    }

    public void RegisterNailSlot(GameObject slot)
    {
        registeredSlots.Add(slot);
    }

    public void ClearAllNailSlots()
    {
        foreach (var slot in registeredSlots)
        {
            if (slot != null)
                Destroy(slot);
        }
        registeredSlots.Clear();
    }


}
