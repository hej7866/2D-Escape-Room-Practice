using UnityEngine;
using System.Collections;

public class SlideZoneController : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트 참조
    public LayerMask wallLayer; // 벽 레이어 마스크

    private PlayerAction playerAction;
    private float tileSize = 1f; // 타일 크기
    private bool isMoving = false; // 이동 중인지 확인
    private Vector2Int currentGridPosition; // 현재 그리드 위치
    private Vector2Int moveDirection; // 이동 방향 (정수 좌표)
    private Vector3 targetPosition; // 목표 위치

    void Awake()
    {
        // 필요한 컴포넌트를 초기화
        playerAction = player.GetComponent<PlayerAction>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // SlideZone에 들어가면 PlayerAction 비활성화, SlideZoneController 활성화
            playerAction.enabled = false;
            this.enabled = true;

            // 캐릭터를 타일 중앙에 위치시키기
            CenterCharacterOnGrid();

            // 캐릭터 크기 조정
            player.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SlideZone"))
        {
            // SlideZone을 벗어나면 PlayerAction 활성화, SlideZoneController 비활성화
            playerAction.enabled = true;
            this.enabled = false;

            // 캐릭터 크기 복구
            player.transform.localScale = Vector3.one;
        }
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveDirection = Vector2Int.up;
                StartCoroutine(MoveContinuous());
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveDirection = Vector2Int.down;
                StartCoroutine(MoveContinuous());
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveDirection = Vector2Int.left;
                StartCoroutine(MoveContinuous());
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveDirection = Vector2Int.right;
                StartCoroutine(MoveContinuous());
            }
        }
    }

    private IEnumerator MoveContinuous()
    {
        isMoving = true;

        while (true)
        {
            // 다음 그리드 위치 계산
            Vector2Int nextGridPosition = currentGridPosition + moveDirection;

            // 다음 위치로의 이동 가능 여부 확인 (벽 충돌 검사)
            Vector3 nextWorldPosition = new Vector3(nextGridPosition.x * tileSize, nextGridPosition.y * tileSize, player.transform.position.z);
            Collider2D hitCollider = Physics2D.OverlapCircle(nextWorldPosition, 0.1f, wallLayer);

            if (hitCollider != null)
            {
                // 벽에 충돌하면 이동 중지
                isMoving = false;
                yield break;
            }

            // 이동
            targetPosition = nextWorldPosition;
            float elapsedTime = 0f;
            float moveDuration = 0.1f; // 이동 속도 조절
            Vector3 startPosition = player.transform.position;

            while (elapsedTime < moveDuration)
            {
                player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 위치를 정확히 목표 위치로 설정
            player.transform.position = targetPosition;
            currentGridPosition += moveDirection;

            // 다음 프레임까지 대기
            yield return null;
        }
    }

    private void CenterCharacterOnGrid()
    {
        // 캐릭터 위치를 타일 중앙에 맞추는 함수
        Vector3 position = player.transform.position;
        currentGridPosition.x = Mathf.RoundToInt(position.x / tileSize);
        currentGridPosition.y = Mathf.RoundToInt(position.y / tileSize);
        position.x = currentGridPosition.x * tileSize;
        position.y = currentGridPosition.y * tileSize;
        player.transform.position = position;
    }
}
