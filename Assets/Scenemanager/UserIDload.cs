using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserIDload : MonoBehaviour
{
    public Text IDtext;

    // Start is called before the first frame update
    void Start()
    {
        string playerID = PlayerPrefs.GetString("PlayerID","");
        IDtext.text = playerID;
    }

}
