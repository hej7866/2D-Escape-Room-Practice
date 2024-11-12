using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    public BoxCollider2D doorCollider; // 문 충돌을 관리하는 Collider
    public CapsuleCollider2D interactionCollider; // 상호작용을 위한 작은 Trigger 
    private bool isPassword = false; // 비밀번호가 맞는지 여부
    public Portal portal;
    public PasswordUI passwordUI;

    private void Start()
    {
        if (doorCollider != null)
        {
            doorCollider.isTrigger = false; // 문 Collider는 처음에 닫힌 상태로 시작
        }
        
        if (interactionCollider != null)
        {
            interactionCollider.isTrigger = true; // 상호작용 Collider는 항상 Trigger 상태
        }
    }

    // 비밀번호 확인 함수 (외부에서 호출)
    public void CheckPassword(string inputPassword)
    {

        if (inputPassword == passwordUI.correctPassword)
        {
            isPassword = true;
            UnlockDoor();
        }
        else
        {
            Debug.Log("비밀번호가 틀렸습니다.");
        }
    }

    // 문을 해금하는 함수
    private void UnlockDoor()
    {
        if (isPassword && doorCollider != null)
        {
            portal.enabled = true;
            Debug.Log("문이 열렸습니다.");

            passwordUI.HidePasswordUI();
            Time.timeScale = 1f;
        }
    }
}
