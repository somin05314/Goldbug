using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int CollectedStars = 0;
    public int TotalStars { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        TotalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        CollectedStars = 0;
        UIManager.Instance.UpdateStarUI(CollectedStars, TotalStars);
    }


    public void CollectStar()
    {
        CollectedStars++;
        UIManager.Instance.UpdateStarUI(CollectedStars, TotalStars);
        Debug.Log("�� ȹ��! ���� ��: " + CollectedStars);
        // UI ���� �� �߰� �۾� ����
    }

    public void CheckGoalReached()
    {
        if (CollectedStars >= TotalStars)
        {
            UIManager.Instance.ShowClearPopup();
            Debug.Log("Ŭ���� ����!");
            // ���� ó��
        }
        else
        {
            Debug.Log("���� �� �� ���� - ����!");
            // ���� ó��
        }
    }

    public void Fail(string reason)
    {
        Debug.Log($"����: {reason}");

        // UI ���� â ����
        UIManager.Instance.ShowFailPopup(reason);

        // ����, �ִϸ��̼� � ���⼭ ó��

        Invoke(nameof(ResetStage), 3f);
    }

    public void ResetStage()
    {
        // ���� �� �ٽ� �ε�
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

