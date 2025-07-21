using UnityEngine;
using UnityEngine.UI; // �� UI ��Ʈ�ѿ�
using TMPro;

public enum BuildMode
{
    Placement,
    Simulation
}

public class BuildModeManager : MonoBehaviour
{
    public static BuildModeManager Instance;

    public BuildMode currentMode = BuildMode.Placement;

    [Header("UI")]
    public Button modeSwitchButton;
    public TextMeshProUGUI modeLabelText;

    // ��� ��ȯ �̺�Ʈ
    public delegate void ModeChanged();
    public event ModeChanged OnEnterPlacement;
    public event ModeChanged OnEnterSimulation;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        modeSwitchButton.onClick.AddListener(ToggleMode);
        UpdateModeUI(); // �ʱ� UI ����
    }

    void ToggleMode()
    {
        if (currentMode == BuildMode.Placement)
            SwitchToSimulation();
        else
            SwitchToPlacement();
    }

    public bool IsPlacementMode()
    {
        return currentMode == BuildMode.Placement;
    }

    public void SwitchToSimulation()
    {
        currentMode = BuildMode.Simulation;
        OnEnterSimulation?.Invoke();
        UpdateModeUI();
    }


    public void SwitchToPlacement()
    {
        currentMode = BuildMode.Placement;
        OnEnterPlacement?.Invoke();
        UpdateModeUI();
    }


    void UpdateModeUI()
    {
        if (modeLabelText != null)
        {
            modeLabelText.text = $"Mode: {currentMode}";
        }

        if (modeSwitchButton != null)
        {
            var buttonText = modeSwitchButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = currentMode == BuildMode.Placement ? "Simulation" : "Make";
            }
        }
    }
}

