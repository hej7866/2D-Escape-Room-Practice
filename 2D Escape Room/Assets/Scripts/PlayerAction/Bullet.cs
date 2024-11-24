using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerAttack playerAttack; // 플레이어의 공격 정보를 가져오기 위해 선언

    void Start()
    {
        // 플레이어의 PlayerAttack 컴포넌트 참조 (태그를 이용해 찾음)
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerAttack = player.GetComponent<PlayerAttack>();
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 객체를 찾을 수 없습니다.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌하지 않으면 총알 제거
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        // 보스와 충돌 시
        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>(); // Boss 컴포넌트 가져오기
            if (boss != null && playerAttack != null) // null 검사
            {
                boss.TakeDamage(playerAttack.attackDamage); // 보스에게 피해를 입힘
                Destroy(gameObject); // 총알 제거
            }
            else
            {
                Debug.LogWarning("Boss 또는 PlayerAttack이 null입니다.");
            }
        }
    }
}
