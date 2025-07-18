using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI starText;

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
        starText.text = $"{collected} / {total}";
    }

    public void ShowFailPopup(string reason)
    {
        Debug.Log("ÆË¾÷");
    }

    public void ShowClearPopup()
    {
        clearPanel.SetActive(true);
    }
}
