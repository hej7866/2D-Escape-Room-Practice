using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorIndex; // 문 인덱스 (정답인지 확인하기 위해 사용)
    public MazeManager mazeManager; // MazeManager를 Unity Inspector에서 할당
    private bool isPlayerNearby = false; // 플레이어가 문 근처에 있는지 확인


    private void Update()
    {
        // 플레이어가 문 근처에 있고, 상호작용 키(E)를 눌렀을 때 상호작용 실행
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            bool isCorrect = mazeManager.CheckCorrectDoor(doorIndex);
            GameObject player = GameObject.FindWithTag("Player");
            if (isCorrect)
            {
                Debug.Log("정답 문을 선택했습니다! 다음 단계로 넘어갑니다.");
                player.transform.position = mazeManager.startPosition.position; // 처음 위치로 이동
            }
            else
            {
                Debug.Log("오답 문입니다. 처음 단계로 돌아갑니다.");
                if (player != null)
                {
                    player.transform.position = mazeManager.startPosition.position; // 처음 위치로 이동
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // 플레이어가 문 근처에 있음
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false; // 플레이어가 문에서 멀어짐
        }
    }
}
