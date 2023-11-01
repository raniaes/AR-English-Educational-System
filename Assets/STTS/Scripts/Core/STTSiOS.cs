using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STTSCore.Utility;
using System.Runtime.InteropServices;
using AOT;

namespace STTSCore.Engine
{
    public class STTSiOS : STTS
    {
        protected delegate void sttCallbackListener(string result);
        protected delegate void errorCallbackListener(string errorDesc);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void STTS_Init(string lang);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void STTS_StartSTT();

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void STTS_StopSTT();

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void STTS_ReleaseSTTS();

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void RegisterSTTSCallback(sttCallbackListener funcPtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void RegisterErrorCallback(errorCallbackListener funcPtr);

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
#endif
        private static extern void STTS_StartTTS(string lang, string text, float speed, float pitch);


        private STTStatus status = STTStatus.None;

        //public override void init()
        //{
        //    init(STTSLangTypes.EN_USA);
        //}

        public override void init(string langType, string gameObjName)
        {
            init(langType);
        }

        private void init(string langType)
        {
            STTS_Init(langType);

            RegisterSTTSCallback(new sttCallbackListener(STTListener));
            RegisterErrorCallback(new errorCallbackListener(ErrorListener));
        }

        // STT
        public override string StartSpeechToText()
        {
            if (status == STTStatus.None || status == STTStatus.Stop)
                STTS_StartSTT();

            status = STTStatus.Record;

            return null;
        }

        public override void StopSpeechToText()
        {
            Debug.Log("[Unity iOS]Stop Speech To TEXT");

            if (status == STTStatus.None || status == STTStatus.Record)
                STTS_StopSTT();

            status = STTStatus.Stop;
        }

        public override void ReleaseSTTS()
        {
            if (status != STTStatus.Stop)
                STTS_StopSTT();

            STTS_ReleaseSTTS();
        }

        // TTS
        public override void StartTextToSpeech(string lang, string text, float speed, float pitch)
        {
            STTS_StartTTS(lang, text, speed, pitch);
        }


        [MonoPInvokeCallback(typeof(sttCallbackListener))]
        static void STTListener(string result)
        {
            Debug.Log("[STT] Result: " + result);

            onSTTEvent(result);
        }

        [MonoPInvokeCallback(typeof(sttCallbackListener))]
        static void ErrorListener(string errorDesc)
        {
            Debug.Log("[STT] Error: " + errorDesc);

            onErrorEvent(errorDesc);
        }
    }
}