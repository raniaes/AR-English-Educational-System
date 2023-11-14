using UnityEngine;
using UnityEngine.UI;

public class PasswordInput : MonoBehaviour
{
    public InputField passwordInputField;

    private void Start()
    {
        passwordInputField.contentType = InputField.ContentType.Password;
        passwordInputField.asteriskChar = '*';
    }
}
