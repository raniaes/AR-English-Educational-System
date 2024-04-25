using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioclick : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudio()
    {

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
