using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLimit = 30f;
    private float currentTime;

    private void Start()
    {
        currentTime = timeLimit;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            GameManager.Instance.Fail("시간 초과");
        }
    }
}
