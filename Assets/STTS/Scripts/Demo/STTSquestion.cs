using System.Collections;
using System.Collections.Generic;
using STTSCore.Engine;
using STTSCore.Utility;
using UnityEngine;
using UnityEngine.UI;

public class STTSquestion : MonoBehaviour, STTSCallback
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private STTS stts;
    public Text sttResultText;
    private string answer;


    void Start()
    {
        answer = "Flying";
        audioSource = GetComponent<AudioSource>();
        stts = STTSFactory.GetInstance();

        if (stts != null)
        {
            stts.init(STTSLangTypes.EN_USA, gameObject.name);
            STTS.onSTTEvent += STTResultCallback;
            STTS.onErrorEvent += ErrorCallback;
        }

    }

    public void PlayAudio()
    {
        sttResultText.color = Color.black;

        sttResultText.text = "Answer";

        audioSource.clip = audioClip;
        audioSource.Play();

        Invoke("StartSTT", audioClip.length + 0.5f);
    }

    private void StartSTT()
    {
        if (stts != null)
        {
            stts.StartSpeechToText();
        }
    }

    // STT Result Event
    public void STTResultCallback(string result)
    {
        Debug.Log("[STT] Result: " + result);
        if (result != null)
        {
            if (result.ToLower() == answer.ToLower())
            {
                // 다음 음성 출력을 여기에 추가하세요.
                sttResultText.text = answer;
                sttResultText.color = Color.green;
                Debug.Log("User asked for coffee. Playing next audio...");

            }
            else
            {
                // 다른 음성 출력을 여기에 추가
                sttResultText.text = result;
                sttResultText.color = Color.red;

                //Invoke("StartSTT", 0.5f);
            }
        }
    }

    // Error Event
    public void ErrorCallback(string error)
    {
        Debug.Log("[Exception] Error: " + error);
        sttResultText.color = Color.red;
    }
}
