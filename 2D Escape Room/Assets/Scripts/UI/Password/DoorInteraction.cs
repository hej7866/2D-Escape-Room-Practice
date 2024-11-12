using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject passwordUIPanel; // 비밀번호 입력 UI 패널
    public Portal portal;
    private bool isPlayerNearby = false; // 플레이어가 문 근처에 있는지 확인

    private void Update()
    {
        // 플레이어가 근처에 있고, 특정 키(E)를 눌렀을 때 UI 활성화
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ShowPasswordUI();
            if(portal.enabled == false)
            {
                Time.timeScale = 0f;
            }
        }

        // Q 키를 눌렀을 때 UI 비활성화
        if (passwordUIPanel.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            HidePasswordUI();
            Time.timeScale = 1f;
        }
    }

    private void ShowPasswordUI()
    {
        // 비밀번호 입력 UI 활성화
        passwordUIPanel.SetActive(true);
    }

    private void HidePasswordUI()
    {
        // 비밀번호 입력 UI 비활성화
        passwordUIPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // 플레이어가 문에 가까이 있음
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
