using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

using STTSCore.Engine;
using STTSCore.Utility;

public class STTSDemo : MonoBehaviour, STTSCallback
{
    public Text sttResultText;

    private STTS stts;

    // Start is called before the first frame update
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        stts = STTSFactory.GetInstance();

        if (stts != null)
        {
            stts.init(STTSLangTypes.EN_USA, gameObject.name);

            // for ios
            STTS.onSTTEvent += STTResultCallback;
            STTS.onErrorEvent += ErrorCallback;
        }
    }

    // STT Event
    public void STTEvent()
    {
        if (stts != null)
        {
            stts.StartSpeechToText();
        }            
    }

    public void STTStopEvent()
    {
        if (stts != null)
            stts.StopSpeechToText();
    }

    private void OnDisable()
    {
        if (stts != null)
            stts.ReleaseSTTS();
    }


    // STT Result Event
    public void STTResultCallback(string result)
    {
        Debug.Log("[STT] Result: " + result);
        if (result != null)
        {
            //sttResultText.text = result;
            if (result.ToLower() == sttResultText.text.ToLower())
            {
                sttResultText.color = Color.green;
            }
            else
            {
                sttResultText.color = Color.red;
                //Invoke("STTEvent", 0.5f);
            }
        }
    }

    // Error Event
    public void ErrorCallback(string error)
    {
        Debug.Log("[Exception] Error: " + error);
        sttResultText.color = Color.red;
        //Invoke("STTEvent", 0.5f);
    }
}
