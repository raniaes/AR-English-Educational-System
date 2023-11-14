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
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
