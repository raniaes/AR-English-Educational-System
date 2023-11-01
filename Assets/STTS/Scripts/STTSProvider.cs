using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

using STTSCore.Engine;
using STTSCore.Utility;

public class STTSProvider : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
