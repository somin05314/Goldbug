using System.Collections.Generic;
using UnityEngine;

public class CurveToolNailPlacer : MonoBehaviour
{
    public GameObject nailSlotPrefab;
    public float spacing = 1.5f; // �� ���� (���� ����)

    private CurveTool curveTool;
    private NailableObject nailable;

    private void Awake()
    {
        curveTool = GetComponent<CurveTool>();
        nailable = GetComponent<NailableObject>();
    }

    public void UpdateNailSlots()
    {
        Debug.Log("������ �����");

        if (curveTool == null || nailSlotPrefab == null || nailable == null)
        {
            Debug.LogWarning("�ʼ� ������Ʈ ����");
            return;
        }

        nailable.ClearAllNailSlots();

        Vector3[] points = curveTool.GetCenterPoints();
        if (points.Length < 2)
            return;

        // 1. � ��ü ���� ���� �� ���׸�Ʈ ���� ����
        float[] segmentLengths = new float[points.Length - 1];
        float totalLength = 0f;

        for (int i = 1; i < points.Length; i++)
        {
            float len = Vector2.Distance(points[i - 1], points[i]);
            segmentLengths[i - 1] = len;
            totalLength += len;
        }

        // 2. �߾� ��ġ���� spacing �������� �¿� ��ġ�� ��ǥ �Ÿ� ���
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

        targetDistances.Sort(); // ���ʺ��� ������� ����

        // 3. � ���󰡸� ��ǥ �Ÿ� ��ġ�� �� ���� ��ġ
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

                Debug.Log($"�� ��ġ: {pos} (� �Ÿ� {targetDistances[targetIndex]:F2})");
                targetIndex++;
            }

            walked += segmentLength;
        }
    }






}
