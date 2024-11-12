using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenDocument : MonoBehaviour
{
    public GameObject puzzlePanel; // 숨겨진 패널 (예: 퍼즐 패널)
    private bool isPlayerInRange = false; // 플레이어가 상호작용 범위에 있는지 여부

    void Start()
    {
        // 시작 시 숨겨진 패널을 비활성화
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
    }

    void Update()
    {
        // 플레이어가 범위 내에 있고 E 키를 누르면 숨겨진 패널 활성화
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenHiddenPanel();
        }

        // 숨겨진 패널이 활성화되어 있을 때 Q 키를 눌러 닫기
        if (puzzlePanel.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            CloseHiddenPanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 상호작용 범위를 벗어남
            isPlayerInRange = false;
        }
    }

    private void OpenHiddenPanel()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true); // 숨겨진 패널 활성화
            Time.timeScale = 0f; // 게임 진행 멈춤
        }
    }

    private void CloseHiddenPanel()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false); // 숨겨진 패널 비활성화
            Time.timeScale = 1f; // 게임 진행 재개
        }
    }
}
