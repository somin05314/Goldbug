using UnityEngine;

public class NailSlot : MonoBehaviour
{
    private NailableObject owner;
    private Transform slotTransform;
    public void Initialize(NailableObject ownerObj, Transform slot)
    {
        owner = ownerObj;
        slotTransform = slot;
    }

    private void Start()
    {
        if (owner == null)
        {
            var parent = GetComponentInParent<NailableObject>();
            if (parent != null)
            {
                owner = parent;
                slotTransform = transform;
            }
        }
    }


    private void OnMouseDown()
    {
        Debug.Log("´­¸²");
        if (!BuildModeManager.Instance.IsPlacementMode())
            return;

        if (!NailManager.Instance.CanPlaceNail())
            return;

        owner.NailAt(slotTransform);
    }
}
