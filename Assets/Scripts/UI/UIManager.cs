using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Star UI")]
    [SerializeField] private TextMeshProUGUI starText;

    [Header("Nail UI")]
    [SerializeField] private TextMeshProUGUI nailText;

    [Header("Panels")]
    [SerializeField] private GameObject clearPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateStarUI(int collected, int total)
    {
        if (starText != null)
            starText.text = $"{collected} / {total}";
    }

    public void UpdateNailUI(int used, int max)
    {
        if (nailText != null)
            nailText.text = $"Nail: {used} / {max}";
    }

    public void ShowFailPopup(string reason)
    {
        Debug.Log("�˾�: " + reason);
        // ���� UI ���� ������ ���⿡ �߰�
    }

    public void ShowClearPopup()
    {
        if (clearPanel != null)
            clearPanel.SetActive(true);
    }
}
