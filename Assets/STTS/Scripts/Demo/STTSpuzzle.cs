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

        // 단어를 무작위로 선택하여 문장 생성
        string sentence = "";
        for (int i = 0; i < shuffledWords.Length; i++)
        {
            sentence += shuffledWords[i] + " ";
        }

        sentence = sentence.Trim(); // 문자열 앞뒤의 공백 제거

        // 생성한 문장과 answer 값을 비교하여 같은 경우 다시 생성
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

    // 배열을 무작위로 섞는 함수
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
