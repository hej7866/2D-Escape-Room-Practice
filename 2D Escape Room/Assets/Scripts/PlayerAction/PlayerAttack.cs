using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트
    public int attackDamage; // 공격 데미지

    // 플레이어 방향 관리
    public string playerDirection; // 기본값: front

    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    public float bulletSpeed = 10f; // 총알 속도

    private List<string> pressedDirections; // 현재 눌려있는 방향키 목록
    public PlayerAction playerAction;


    void Awake()
    {
        Animator animator = GetComponent<Animator>();
    }

    void Start()
    {
        firePoint.localPosition += new Vector3(0, -0.2f, 0);
        pressedDirections = new List<string>();
    }


    void Update()
    {
        // 공격 입력 감지
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Attack();
            Shoot(); // 총알 발사
        }
        SetDirection();
    }

    void Attack()
    {
        // 플레이어 방향에 따른 애니메이션 트리거 설정
        switch (playerDirection)
        {
            case "front":
                animator.SetTrigger("attack_front");
                break;
            case "back":
                animator.SetTrigger("attack_back");
                break;
            case "right":
                animator.SetTrigger("attack_right");
                break;
            case "left":
                animator.SetTrigger("attack_left");
                break;
        }
    }

    void Shoot()
    {
        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 총알의 Rigidbody2D 가져오기
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Transform transform = bullet.GetComponent<Transform>();

        // 플레이어 방향에 따라 총알 발사
        switch (playerDirection)
        {
            case "front":
                rb.velocity = Vector2.down * bulletSpeed;
                transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case "back":
                rb.velocity = Vector2.up * bulletSpeed;
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case "right":
                rb.velocity = Vector2.right * bulletSpeed;
                break;
            case "left":
                rb.velocity = Vector2.left * bulletSpeed;
                transform.eulerAngles = new Vector3(0, 0, 180);
                break;
        }
    }

    public void SetDirection()
    {
        // 키 눌림 처리
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pressedDirections.Add("back");
            playerDirection = "back";
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pressedDirections.Add("front");
            playerDirection = "front";
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            pressedDirections.Add("right");
            playerDirection = "right";
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pressedDirections.Add("left");
            playerDirection = "left";
        }

        // 키 뗌 처리
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            pressedDirections.Remove("back");
            UpdatePlayerDirection();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            pressedDirections.Remove("front");
            UpdatePlayerDirection();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            pressedDirections.Remove("right");
            UpdatePlayerDirection();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            pressedDirections.Remove("left");
            UpdatePlayerDirection();
        }
    }

    void UpdatePlayerDirection()
    {
        if (pressedDirections.Count > 0)
        {
            // 마지막으로 눌린 방향키로 설정
            playerDirection = pressedDirections[pressedDirections.Count - 1];
        }
    }
}
