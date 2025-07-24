using System.Collections.Generic;
using UnityEngine;

public class NailableObject : MonoBehaviour
{
    public GameObject nailSlotPrefab;      // 슬롯 표시 프리팹
    public Rigidbody2D targetBody;         // 못이 고정될 대상
    private List<GameObject> registeredSlots = new List<GameObject>();

    // 외부에서 로컬 위치를 넘겨 받아 슬롯 생성
    public void CreateNailSlotAt(Vector2 localPos)
    {
        Vector2 worldPos = transform.TransformPoint(localPos);

        GameObject slot = Instantiate(nailSlotPrefab, worldPos, Quaternion.identity, transform);
        var nailSlot = slot.GetComponent<NailSlot>();
        if (nailSlot != null)
        {
            nailSlot.Initialize(this, transform); // 못 설치 시 부모 기준으로 처리
        }
    }

    // 슬롯에서 호출되는 못 설치 함수
    public void NailAt(Transform slotTransform)
    {
        // 못 프리팹 생성
        GameObject nailObj = Instantiate(NailManager.Instance.nailPrefab, slotTransform.position, Quaternion.identity);

        // HingeJoint2D 추가
        var joint = nailObj.AddComponent<HingeJoint2D>();
        joint.connectedBody = targetBody;

        // 못 위치를 기준으로 회전하게 설정
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero; // 못의 중앙
        joint.connectedAnchor = targetBody.transform.InverseTransformPoint(slotTransform.position);

        joint.enableCollision = false; // 못과 물체 사이 충돌은 비활성화

        // 회전을 느리게 만들기 위한 모터 설정
        joint.useMotor = true;
        JointMotor2D motor = new JointMotor2D
        {
            motorSpeed = 0f, // 중립 속도. (회전을 강제로 시키지 않음)
            maxMotorTorque = 10f // 회전에 대한 저항. 높을수록 덜 흔들림, 더 천천히 움직임
        };
        joint.motor = motor;

        // 못 개수 차감
        NailManager.Instance.UseNail();

        // 슬롯 제거
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
