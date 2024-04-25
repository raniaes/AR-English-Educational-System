using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyboard : MonoBehaviour
{
    public InputField inputField;
    public InputField cancelfocus;

    public void ActivateKeyboard()
    {
        cancelfocus.Select();
        inputField.DeactivateInputField();
        inputField.Select();
    }
}
