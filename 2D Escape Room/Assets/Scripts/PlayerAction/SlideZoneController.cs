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

    public CapsuleCollider2D capsuleColliderV; // Vertical 캡슐 콜라이더
    public CapsuleCollider2D capsuleColliderH; // Horizontal 캡슐 콜라이더
    public CompositeCollider2D compositeCollider; // 충돌용 콜라이더

    public bool isColliding;

    void Awake()
    {
        // 필요한 컴포넌트를 초기화
        playerAction = player.GetComponent<PlayerAction>();
        rb2d = player.GetComponent<Rigidbody2D>();

        // 초기 콜라이더 설정
        EnableVerticalCollider(); // 기본적으로 Vertical Collider 활성화
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // PlayerAction 비활성화, SlideZoneController 활성화
            playerAction.enabled = false;
            this.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // PlayerAction 활성화, SlideZoneController 비활성화
            playerAction.enabled = true;
            this.enabled = false;

            // Rigidbody2D 설정 변경
            rb2d.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
        }
    }

    void Update()
    {
        if (capsuleColliderV != null && capsuleColliderH != null && compositeCollider != null)
        {
            isColliding = capsuleColliderV.IsTouching(compositeCollider) || capsuleColliderH.IsTouching(compositeCollider);
            Debug.Log($"IsColliding: {isColliding}");

            if (isColliding)
            {
                Debug.Log("Collider와 CompositeCollider가 충돌 중입니다.");
                SlideMove();
            }
            else
            {
                Debug.Log("충돌 없음.");
            }
        }
        else
        {
            Debug.LogWarning("Collider가 제대로 연결되지 않았습니다.");
        }

        Debug.Log("Update - rb2d.velocity: " + rb2d.velocity);
    }

    void SlideMove()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
            EnableVerticalCollider(); // 위쪽으로 이동 시 Vertical Collider 활성화
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
            EnableVerticalCollider(); // 아래쪽으로 이동 시 Vertical Collider 활성화
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
            EnableHorizontalCollider(); // 왼쪽으로 이동 시 Horizontal Collider 활성화
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
            EnableHorizontalCollider(); // 오른쪽으로 이동 시 Horizontal Collider 활성화
        }
    }

    void FixedUpdate()
    {
        // rb2d.velocity 디버그 출력
        Debug.Log("FixedUpdate - rb2d.velocity: " + rb2d.velocity);
        rb2d.MovePosition(rb2d.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    // Vertical Collider 활성화, Horizontal Collider 비활성화
    void EnableVerticalCollider()
    {
        if (capsuleColliderV != null) capsuleColliderV.enabled = true;
        if (capsuleColliderH != null) capsuleColliderH.enabled = false;
    }

    // Horizontal Collider 활성화, Vertical Collider 비활성화
    void EnableHorizontalCollider()
    {
        if (capsuleColliderH != null) capsuleColliderH.enabled = true;
        if (capsuleColliderV != null) capsuleColliderV.enabled = false;
    }
}
