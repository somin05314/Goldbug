using System.Linq;
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

        UIManager.Instance.ShowFailPopup(reason);

        // 씬 리로드 대신 시뮬레이션 초기화
        Invoke(nameof(ResetSimulation), 2f);
    }

    public void ResetSimulation()
    {
        // 배치 모드로 전환
        BuildModeManager.Instance.SwitchToPlacement();

        // 플레이어, 구슬, 움직이는 물체 등 초기 위치로 복귀
        foreach (var resettable in FindObjectsOfType<MonoBehaviour>().OfType<IResettable>())
        {
            resettable.ResetToInitialState();
        }

        // 별 개수 초기화
        CollectedStars = 0;
        UIManager.Instance.UpdateStarUI(CollectedStars, TotalStars);
    }



    public void ResetStage()
    {
        // 현재 씬 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

