using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class SlideZoneController : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트 참조
    private PlayerAction playerAction;
    private Rigidbody2D rb2d;

    private float moveSpeed = 5f; // 이동 속도
    private Vector2 moveDirection = Vector2.zero; // 이동 방향

    public Animator animator; // Animator 참조

    public CapsuleCollider2D capsuleCollider;
    public CircleCollider2D circleCollider;
    public CompositeCollider2D compositeCollider;

    public bool isColliding;

    // 스프라이트 관련 변수
    public SpriteRenderer spriteRenderer; // 캐릭터 스프라이트 렌더러
    public Sprite[] directionSprites; // 방향별 스프라이트 배열 (0: 위, 1: 아래, 2: 왼쪽, 3: 오른쪽)

    void Awake()
    {
        // 필요한 컴포넌트를 초기화
        playerAction = player.GetComponent<PlayerAction>();
        rb2d = player.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // PlayerAction 비활성화, SlideZoneController 활성화
            playerAction.enabled = false;
            this.enabled = true;
            circleCollider.enabled = true;

            animator.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // PlayerAction 활성화, SlideZoneController 비활성화
            playerAction.enabled = true;
            this.enabled = false;
            circleCollider.enabled = false;

            animator.enabled = true;

            // Rigidbody2D 설정 초기화
            rb2d.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
        }
    }

    void Update()
    {
        if (circleCollider != null && compositeCollider != null)
        {
            isColliding = circleCollider.IsTouching(compositeCollider);
            Debug.Log($"IsColliding: {isColliding}");

            if (isColliding && rb2d.velocity == Vector2.zero)
            {
                Debug.Log("CircleCollider2D와 CompositeCollider2D가 충돌 중입니다.");
                SlideMove();
            }
            else
            {
                Debug.Log("충돌 없음.");
            }
        }
        else
        {
            Debug.LogWarning("CircleCollider2D 또는 CompositeCollider2D가 제대로 연결되지 않았습니다.");
        }

        Debug.Log("Update - rb2d.velocity: " + rb2d.velocity);
    }

    void SlideMove()
    {
        // 방향 입력에 따라 moveDirection 설정 및 스프라이트 변경
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
            SetSprite(0); // 위쪽 방향 스프라이트
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
            SetSprite(1); // 아래쪽 방향 스프라이트
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
            SetSprite(2); // 왼쪽 방향 스프라이트
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
            SetSprite(3); // 오른쪽 방향 스프라이트
            spriteRenderer.flipX = false;
        }
    }

    void SetSprite(int index)
    {
        Debug.Log($"SetSprite 호출됨 - Index: {index}");

        if (spriteRenderer != null && directionSprites.Length > index && directionSprites[index] != null)
        {
            spriteRenderer.sprite = directionSprites[index];
            Debug.Log($"Sprite 변경 성공 - New Sprite: {directionSprites[index].name}");
        }
        else
        {
            Debug.LogWarning("스프라이트가 배열에서 누락되었거나 SpriteRenderer가 할당되지 않았습니다.");
        }
    }

    void FixedUpdate()
    {
        // rb2d.velocity를 사용하여 이동 처리
        rb2d.velocity = moveDirection * moveSpeed;

        // 이동이 멈췄을 경우 방향 초기화
        if (moveDirection != Vector2.zero && rb2d.velocity.magnitude < 0.1f)
        {
            moveDirection = Vector2.zero;
        }

        Debug.Log($"FixedUpdate - Velocity: {rb2d.velocity}");
    }
}
