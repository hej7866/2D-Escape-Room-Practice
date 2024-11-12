using UnityEngine;
using UnityEngine.UI;

public class PasswordInput : MonoBehaviour
{
    public InputField passwordInputField;

    private void Start()
    {
        // 최대 글자 수 4로 제한
        passwordInputField.characterLimit = 4;

        // 숫자만 입력 가능하도록 설정
        passwordInputField.contentType = InputField.ContentType.IntegerNumber;
    }
}
