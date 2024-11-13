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

    public CircleCollider2D circleCollider;
    public CapsuleCollider2D capsuleCollider;
    public CompositeCollider2D compositeCollider;

    public bool isColliding;

    void Awake()
    {
        // 필요한 컴포넌트를 초기화
        playerAction = player.GetComponent<PlayerAction>();
        rb2d = player.GetComponent<Rigidbody2D>();

        // 시작 시 캡슐 콜라이더 활성화, 박스 콜라이더 비활성화
        if (capsuleCollider != null) capsuleCollider.enabled = true;
        if (circleCollider != null) circleCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // PlayerAction 비활성화, SlideZoneController 활성화
            playerAction.enabled = false;
            this.enabled = true;

            // 캡슐 콜라이더 비활성화, 박스 콜라이더 활성화
            if (capsuleCollider != null) capsuleCollider.enabled = false;
            if (circleCollider != null) circleCollider.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // PlayerAction 활성화, SlideZoneController 비활성화
            playerAction.enabled = true;
            this.enabled = false;

            // 박스 콜라이더 비활성화, 캡슐 콜라이더 활성화
            if (circleCollider != null) circleCollider.enabled = false;
            if (capsuleCollider != null) capsuleCollider.enabled = true;

            // Rigidbody2D 설정 변경
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

            if (isColliding)
            {
                Debug.Log("CircleCollider2D와 TilemapCollider2D가 충돌 중입니다.");
                SlideMove();
            }
            else
            {
                Debug.Log("충돌 없음.");
            }
        }
        else
        {
            Debug.LogWarning("CircleCollider2D 또는 TilemapCollider2D가 제대로 연결되지 않았습니다.");
        }

        Debug.Log("Update - rb2d.velocity: " + rb2d.velocity);
    }


    void SlideMove()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
        }
    }

    void FixedUpdate()
    {
        // rb2d.velocity 디버그 출력
        Debug.Log("FixedUpdate - rb2d.velocity: " + rb2d.velocity);
        rb2d.MovePosition(rb2d.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
