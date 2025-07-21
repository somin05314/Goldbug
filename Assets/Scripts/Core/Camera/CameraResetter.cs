using UnityEngine;

public class CameraResetter : MonoBehaviour, IResettable
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetToInitialState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}

