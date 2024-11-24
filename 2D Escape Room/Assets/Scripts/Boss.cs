using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 100; // 보스의 체력
    public float attackCooldown = 5f; // 공격 패턴 간격

    private float attackTimer; // 공격 패턴을 위한 타이머


    void Start()
    {
        // 초기화
        attackTimer = attackCooldown;
    }

    void Update()
    {
        // 타이머를 업데이트
        attackTimer -= Time.deltaTime;

        // 공격 패턴 실행
        if (attackTimer <= 0)
        {
            PerformAttackPattern();
            attackTimer = attackCooldown; // 타이머 초기화
        }
    }

    // 공격 패턴 실행
    void PerformAttackPattern()
    {
        int randomPattern = Random.Range(0, 3); // 0~2 범위의 랜덤 숫자 생성

        switch (randomPattern)
        {
            case 0:
                Pattern1();
                break;
            case 1:
                Pattern2();
                break;
            case 2:
                Pattern3();
                break;
        }
    }

    // 공격 패턴 1: 근처에 폭발 생성
    void Pattern1()
    {
        Debug.Log("패턴 1: 근처에 폭발 생성");
        // 폭발 생성 로직 추가
    }

    // 공격 패턴 2: 플레이어를 향해 발사체 발사
    void Pattern2()
    {
        Debug.Log("패턴 2: 플레이어를 향해 발사체 발사");
        // 발사체 생성 및 플레이어를 향한 발사 로직 추가
    }

    // 공격 패턴 3: 맵에 광역 공격
    void Pattern3()
    {
        Debug.Log("패턴 3: 맵에 광역 공격");
        // 맵 특정 영역에 공격 효과 추가
    }

    // 보스 체력 감소
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"보스 체력: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    // 보스 사망
    void Die()
    {
        Debug.Log("보스 사망");
        // 사망 처리 로직 추가 (애니메이션, 파괴, 전리품 등)
        Destroy(gameObject);
    }
}
