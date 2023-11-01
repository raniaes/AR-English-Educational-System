using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserIDsave : MonoBehaviour
{
    public Text IDtext;

    // Start is called before the first frame update
    void Start()
    {
        string playerID = IDtext.text;
        PlayerPrefs.SetString("PlayerID", playerID);
        PlayerPrefs.Save();
    }

}
