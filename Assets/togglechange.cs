using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class togglechange : MonoBehaviour
{
    public Toggle toggle;

    public void changetoggle()
    {
        if (toggle.isOn)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }
}
