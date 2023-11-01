using UnityEngine;
using UnityEngine.UI;

public class AudioPlayOnClick : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
