using System.Collections;
using System.Collections.Generic;
using STTSCore.Engine;
using STTSCore.Utility;
using UnityEngine;
using UnityEngine.UI;

public class STTSpuzzle : MonoBehaviour, STTSCallback
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private STTS stts;
    public Text sttResultText;
    private string answer;
    private string[] words;


    void Start()
    {
        words = new string[4];
        words[0] = "The";
        words[1] = "tiger";
        words[2] = "is";
        words[3] = "running";
        answer = "The tiger is running";
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


        sttResultText.text = GenerateRandomSentence();

        audioSource.clip = audioClip;
        audioSource.Play();

        Invoke("StartSTT", audioClip.length + 0.5f);
    }

    private string GenerateRandomSentence()
    {
        string[] shuffledWords = ShuffleArray(words);

        string sentence = "";
        for (int i = 0; i < shuffledWords.Length; i++)
        {
            sentence += shuffledWords[i] + " ";
        }

        sentence = sentence.Trim();

        while (sentence == answer)
        {
            shuffledWords = ShuffleArray(words);
            sentence = "";
            for (int i = 0; i < shuffledWords.Length; i++)
            {
                sentence += shuffledWords[i] + " ";
            }
            sentence = sentence.Trim();
        }

        return sentence;
    }

    private T[] ShuffleArray<T>(T[] array)
    {
        System.Random random = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
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
                sttResultText.text = answer;
                sttResultText.color = Color.green;
                Debug.Log("User asked for coffee. Playing next audio...");

            }
            else
            {
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
