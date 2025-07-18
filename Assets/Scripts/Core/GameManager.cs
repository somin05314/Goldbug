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
        Debug.Log("별 획득! 현재 수: " + CollectedStars);
        // UI 갱신 등 추가 작업 가능
    }

    public void CheckGoalReached()
    {
        if (CollectedStars >= TotalStars)
        {
            UIManager.Instance.ShowClearPopup();
            Debug.Log("클리어 성공!");
            // 성공 처리
        }
        else
        {
            Debug.Log("별을 다 못 먹음 - 실패!");
            // 실패 처리
        }
    }

    public void Fail(string reason)
    {
        Debug.Log($"실패: {reason}");

        // UI 실패 창 띄우기
        UIManager.Instance.ShowFailPopup(reason);

        // 사운드, 애니메이션 등도 여기서 처리

        Invoke(nameof(ResetStage), 3f);
    }

    public void ResetStage()
    {
        // 현재 씬 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

