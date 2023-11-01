using UnityEngine;
using UnityEngine.UI;

public class PasswordInput : MonoBehaviour
{
    public InputField passwordInputField;

    private void Start()
    {
        // Input Field의 inputType을 Password로 설정
        passwordInputField.contentType = InputField.ContentType.Password;
        // 숨길 문자를 원하는 문자로 설정 (기본값은 '*')
        passwordInputField.asteriskChar = '*';
    }
}
