using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleaudio : MonoBehaviour
{
    public Toggle toggle;
    private AudioSource caccount;

    [SerializeField] private AudioClip[] audioClip;

    void Start()
    {
        caccount = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void audioplay()
    {
        if (toggle.isOn)
        {
            caccount.clip = audioClip[0];
            caccount.Play();
        }
        else
        {
            caccount.clip = audioClip[1];
            caccount.Play();
        }
    }
}
