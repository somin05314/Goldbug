using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // 상대방이 Player 태그인지 확인
        if (other.CompareTag("Player"))
        {
            Collect(); // 나 자신(별)이 먹힌다
        }
    }

    void Collect()
    {
        // 별 수 증가
        GameManager.Instance.CollectStar();

        // 이펙트, 사운드 등 추가 가능

        // 별 사라지기
        Destroy(gameObject);
    }
}

