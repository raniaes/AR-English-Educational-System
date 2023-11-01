using UnityEngine;
using UnityEngine.UI;

public class PasswordInput : MonoBehaviour
{
    public InputField passwordInputField;

    private void Start()
    {
        // Input Field�� inputType�� Password�� ����
        passwordInputField.contentType = InputField.ContentType.Password;
        // ���� ���ڸ� ���ϴ� ���ڷ� ���� (�⺻���� '*')
        passwordInputField.asteriskChar = '*';
    }
}
