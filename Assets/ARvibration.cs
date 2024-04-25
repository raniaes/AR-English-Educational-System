using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using distriqt.plugins.vibration;

public class ARvibration : MonoBehaviour
{
    public void Click()
    {
        Vibration.Instance.Vibrate(100);
    }
}
