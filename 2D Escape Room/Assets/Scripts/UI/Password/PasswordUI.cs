using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordUI : MonoBehaviour
{
    public InputField passwordInputField; // 비밀번호 입력 필드
    public Text feedbackText;             // 피드백 메시지 (비밀번호 오류 등)
    public string correctPassword; // 설정된 올바른 비밀번호
    public DoorUnlock doorUnlockScript;   // DoorUnlock 스크립트 참조

    // 비밀번호 확인 함수
    public void OnPasswordSubmit()
    {
        string enteredPassword = passwordInputField.text;

        if (doorUnlockScript != null)
        {
            doorUnlockScript.CheckPassword(enteredPassword); // 비밀번호 확인
        }

        // 입력 필드 초기화
        passwordInputField.text = "";
    }

    // UI 닫기 함수
    public void HidePasswordUI()
    {
        gameObject.SetActive(false); // Password UI 비활성화
    }
}