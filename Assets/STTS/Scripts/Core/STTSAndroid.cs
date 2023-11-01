using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STTSCore.Utility;

namespace STTSCore.Engine
{
    public class STTSAndroid : STTS
    {
        AndroidJavaObject _pluginInstance;

        public override void init(string langType, string gameObjName)
        {
            var pluginClass = new AndroidJavaClass("com.hankyo.jeong.sttsplugin.UnityPlugin");
            _pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");

            _pluginInstance.Call("InitSTTS", langType, gameObjName);

            // For Test
            //string text = _pluginInstance.Call<string>("getPackageName");
            //Debug.Log("Package Name: " + text);
        }

        public override string StartSpeechToText()
        {
            Debug.Log("[STTS] StartSpeechToText");

            _pluginInstance.Call("StartSpeechToText");

            return null; 
        }

        public override void StartTextToSpeech(string lang, string text, float speed, float pitch)
        {
            Debug.Log("[STTS] StartSpeechToText speed " + "[" + speed + "], pitch " + "[" + pitch + "]");

            if (speed > 2.0f)
                speed = 2.0f;
            else if (speed < 0)
                speed = 0.0f;

            if (pitch > 2.0f)
                pitch = 2.0f;
            else if (pitch < 0)
                pitch = 0.0f;

            _pluginInstance.Call("StartTextToSpeech", text, speed, pitch);
        }

        public void SetErrorCallback()
        {

        }
    }
}