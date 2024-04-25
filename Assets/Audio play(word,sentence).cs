using UnityEngine;
using UnityEngine.UI;

public class AudioPlayOnClick : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
