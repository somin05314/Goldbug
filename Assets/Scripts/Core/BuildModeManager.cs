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
        // 여기에 구슬 떨어뜨리기, 타이머 시작 등 연출 넣어도 됨
    }

    public void SwitchToPlacement()
    {
        currentMode = BuildMode.Placement;
        // 초기화할 게 있으면 여기서 처리
    }
}
