using System.Collections.Generic;
using UnityEngine;

public class CurveToolNailPlacer : MonoBehaviour
{
    public GameObject nailSlotPrefab;
    public float spacing = 1.5f; // 못 간격 (유닛 기준)

    private CurveTool curveTool;
    private NailableObject nailable;

    private void Awake()
    {
        curveTool = GetComponent<CurveTool>();
        nailable = GetComponent<NailableObject>();
    }

    public void UpdateNailSlots()
    {
        Debug.Log("못생성 실행됨");

        if (curveTool == null || nailSlotPrefab == null || nailable == null)
        {
            Debug.LogWarning("필수 컴포넌트 누락");
            return;
        }

        nailable.ClearAllNailSlots();

        Vector3[] points = curveTool.GetCenterPoints();
        if (points.Length < 2)
            return;

        // 1. 곡선 전체 길이 측정 및 세그먼트 길이 저장
        float[] segmentLengths = new float[points.Length - 1];
        float totalLength = 0f;

        for (int i = 1; i < points.Length; i++)
        {
            float len = Vector2.Distance(points[i - 1], points[i]);
            segmentLengths[i - 1] = len;
            totalLength += len;
        }

        // 2. 중앙 위치부터 spacing 간격으로 좌우 배치할 목표 거리 계산
        List<float> targetDistances = new List<float>();
        float center = totalLength / 2f;
        targetDistances.Add(center);

        for (float offset = spacing; center - offset >= 0 || center + offset <= totalLength; offset += spacing)
        {
            if (center - offset >= 0)
                targetDistances.Add(center - offset);
            if (center + offset <= totalLength)
                targetDistances.Add(center + offset);
        }

        targetDistances.Sort(); // 앞쪽부터 순서대로 정렬

        // 3. 곡선 따라가며 목표 거리 위치에 못 슬롯 배치
        float walked = 0f;
        int targetIndex = 0;

        for (int i = 1; i < points.Length && targetIndex < targetDistances.Count; i++)
        {
            Vector2 p0 = points[i - 1];
            Vector2 p1 = points[i];
            float segmentLength = segmentLengths[i - 1];
            Vector2 dir = (p1 - p0).normalized;

            while (targetIndex < targetDistances.Count && walked + segmentLength >= targetDistances[targetIndex])
            {
                float localDist = targetDistances[targetIndex] - walked;
                Vector2 pos = p0 + dir * localDist;

                GameObject slot = Instantiate(nailSlotPrefab, pos, Quaternion.identity, transform);
                nailable.RegisterNailSlot(slot);

                Debug.Log($"못 설치: {pos} (곡선 거리 {targetDistances[targetIndex]:F2})");
                targetIndex++;
            }

            walked += segmentLength;
        }
    }






}
