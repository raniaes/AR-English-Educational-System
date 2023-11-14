using UnityEngine;
using UnityEngine.UI;
using STTSCore.Engine;
using STTSCore.Utility;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class STTSsenario : MonoBehaviour, STTSCallback
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip1;
    [SerializeField] private AudioClip audioClip2;
    [SerializeField] private AudioClip audioClip3;
    [SerializeField] private AudioClip audioClip4;
    private STTS stts;
    public Text sttResultText;
    public Text systemText;
    private string answer1;
    private string answer2;

    public ARSessionOrigin arSessionOrigin;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 screenCenter;

    public GameObject coffeePrefab;
    private GameObject spawnedCoffee;

    void Start()
    {
        answer1 = "can i get a coffee";
        answer2 = "thank you";
        audioSource = GetComponent<AudioSource>();
        stts = STTSFactory.GetInstance();

        if (stts != null)
        {
            stts.init(STTSLangTypes.EN_USA, gameObject.name);
            STTS.onSTTEvent += STTResultCallback;
            STTS.onErrorEvent += ErrorCallback;
        }

        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        raycastManager = arSessionOrigin.GetComponent<ARRaycastManager>();
    }

    public void PlayAudio()
    {
        DestroyCoffee();

        sttResultText.color = Color.black;

        sttResultText.text = "Can I get a coffee";
        systemText.text = "Hello";

        audioSource.clip = audioClip1;
        audioSource.Play();

        Invoke("PlayAudioClip2", audioClip1.length + 0.5f);
    }
    private void PlayAudioClip2()
    {
        systemText.text = "What would you like to order";

        audioSource.clip = audioClip2;
        audioSource.Play();

        Invoke("StartSTT", audioClip2.length + 0.5f);
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
            if (result.ToLower() == answer1.ToLower())
            {
                sttResultText.color = Color.green;
                Debug.Log("User asked for coffee. Playing next audio...");
                Invoke("PlayAudioClip3", 0.5f);
            }
            else if (result.ToLower() == answer2.ToLower())
            {
                sttResultText.color = Color.green;
                Debug.Log("User asked for coffee. Playing next audio...");

                PlayAudioClip4();
            }
            else
            {
                sttResultText.color = Color.red;

                //Invoke("StartSTT", 0.5f);
            }
        }
    }

    private void PlayAudioClip3()
    {
        systemText.text = "Here you are";

        audioSource.clip = audioClip3;
        audioSource.Play();

        sttResultText.text = "Thank you";
        sttResultText.color = Color.black;

        Invoke("StartSTT", audioClip3.length + 0.5f);

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            InstantiateCoffee(hitPose.position);
        }
    }

    private void PlayAudioClip4()
    {
        systemText.text = "Have a good day";

        audioSource.clip = audioClip4;
        audioSource.Play();
    }

    private void InstantiateCoffee(Vector3 position)
    {
        if (coffeePrefab != null && arSessionOrigin != null)
        {
            Vector3 coffeePosition = position + new Vector3(0f, 0f, -0.1f);
            spawnedCoffee = Instantiate(coffeePrefab, coffeePosition, Quaternion.identity);
            spawnedCoffee.transform.SetParent(arSessionOrigin.transform);
        }
    }

    private void DestroyCoffee()
    {
        if (spawnedCoffee != null)
        {
            Destroy(spawnedCoffee);
            spawnedCoffee = null;
        }
    }

    // Error Event
    public void ErrorCallback(string error)
    {
        Debug.Log("[Exception] Error: " + error);
        sttResultText.color = Color.red;
        //Invoke("StartSTT", 0.5f);
    }
}
