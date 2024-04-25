using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialaudio : MonoBehaviour
{
    public Text infotext;
    public string info;
    private AudioSource audioSource;
    
    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudio()
    {
        if (infotext != null)
        {
            infotext.text = info;
        }

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
