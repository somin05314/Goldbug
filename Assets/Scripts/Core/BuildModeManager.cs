using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildMode
{
    Placement,
    Simulation
}

public class BuildModeManager : MonoBehaviour
{
    public static BuildModeManager Instance;

    public BuildMode currentMode = BuildMode.Placement;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (BuildModeManager.Instance.currentMode == BuildMode.Placement)
                BuildModeManager.Instance.SwitchToSimulation();
            else
                BuildModeManager.Instance.SwitchToPlacement();
        }
    }

    public bool IsPlacementMode()
    {
        return currentMode == BuildMode.Placement;
    }

    public void SwitchToSimulation()
    {
        currentMode = BuildMode.Simulation;
        // ���⿡ ���� ����߸���, Ÿ�̸� ���� �� ���� �־ ��
    }

    public void SwitchToPlacement()
    {
        currentMode = BuildMode.Placement;
        // �ʱ�ȭ�� �� ������ ���⼭ ó��
    }
}
