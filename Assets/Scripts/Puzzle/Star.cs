using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // ������ Player �±����� Ȯ��
        if (other.CompareTag("Player"))
        {
            Collect(); // �� �ڽ�(��)�� ������
        }
    }

    void Collect()
    {
        // �� �� ����
        GameManager.Instance.CollectStar();

        // ����Ʈ, ���� �� �߰� ����

        // �� �������
        Destroy(gameObject);
    }
}

