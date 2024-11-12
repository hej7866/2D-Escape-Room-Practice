using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameUIInterface : MonoBehaviour
{
    public GameObject frameUIPanel;  // UI 패널 (액자를 가까이서 보는 인터페이스)
    public GameObject frameTextPanel;
    public Text frameText;
    public Image frameUIImage;       // UI 패널에서 보여줄 액자 이미지
    public Sprite[] frameSprites;    // 여러 액자 스프라이트를 담는 배열
    public DialogueManager dialogueManager; // DialogueManager 참조

    private bool isPlayerInRange = false;
    private int currentFrameIndex = -1; // 현재 상호작용 중인 액자의 인덱스

    void Start()
    {
        // 시작 시 UI 패널 비활성화
        if (frameUIPanel != null)
        {
            frameUIPanel.SetActive(false);
            frameTextPanel.SetActive(false);
        }
    }

    void Update()
    {
        // 플레이어가 상호작용 범위에 있고 E 키를 누르면 UI 패널 활성화
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && currentFrameIndex != -1)
        {
            OpenFrameUI(currentFrameIndex);
        }

        // UI 패널이 활성화되어 있을 때, Q 키를 눌러 닫기
        if (frameUIPanel.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            CloseFrameUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 상호작용할 액자에 닿으면 인덱스 업데이트
            for (int i = 0; i < frameSprites.Length; i++)
            {
                if (gameObject.name == "Frame" + i) // 액자의 이름이 "Frame0", "Frame1" 등의 형식
                {
                    isPlayerInRange = true;
                    currentFrameIndex = i; // 현재 상호작용 중인 액자의 인덱스 설정
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 액자 범위를 벗어나면 상호작용 정보 초기화
            isPlayerInRange = false;
            currentFrameIndex = -1;
        }
    }

    private void OpenFrameUI(int index)
    {
         if (frameUIPanel != null && frameUIImage != null)
        {
            // UI 패널 활성화 및 스프라이트 설정
            frameUIPanel.SetActive(true);
            frameTextPanel.SetActive(true);
            frameUIImage.sprite = frameSprites[index];

            // 액자 설명을 DialogueManager에서 가져와 텍스트 패널에 표시
            if (dialogueManager != null && frameText != null)
            {
                FrameDialogue frameDialogue = dialogueManager.GetFrameDialogueByID(index + 1); // index가 0부터 시작이므로 ID는 +1
                if (frameDialogue != null)
                {
                    frameText.text = frameDialogue.Text;
                }
            }

            // 게임 진행을 멈추기 위해 시간 정지 (선택 사항)
            Time.timeScale = 0f;
        }
    }

    private void CloseFrameUI()
    {
        if (frameUIPanel != null)
        {
            frameTextPanel.SetActive(false); // 액자 설명 패널 활성화
            frameUIPanel.SetActive(false);  // UI 패널 비활성화
            // 게임 진행 재개
            Time.timeScale = 1f;
        }
    }
}


