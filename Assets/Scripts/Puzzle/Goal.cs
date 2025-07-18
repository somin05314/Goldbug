using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 구슬에 "Player" 태그가 있어야 함
        {
            GameManager.Instance.CheckGoalReached();
            other.gameObject.SetActive(false);

        }
    }
}
