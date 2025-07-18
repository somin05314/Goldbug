using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ������ "Player" �±װ� �־�� ��
        {
            GameManager.Instance.CheckGoalReached();
            other.gameObject.SetActive(false);

        }
    }
}
