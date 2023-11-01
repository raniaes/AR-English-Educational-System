using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STTSCore.Utility;

public delegate void EventNotify (string result);

namespace STTSCore.Engine
{
    public interface STTSCallback
    {
        // STT Result
        void STTResultCallback(string result);

        // Error Result
        void ErrorCallback(string errorDescription);
    }

    public abstract class STTS
    {
        //public virtual void init(){}

        //public virtual void init(string langType) { }

        public virtual void init(string langType, string gameObjName) { }

        // STT
        public virtual string StartSpeechToText(){ return null; }

        public virtual void StopSpeechToText() { }

        public virtual void ReleaseSTTS() { }

        // TTS
        public virtual void StartTextToSpeech(string lang, string text, float speed, float pitch) { }

        // Only For iOS
        public static EventNotify onSTTEvent;
        public static EventNotify onErrorEvent;

        protected enum STTStatus
        {
            None = 0,
            Record = 1,
            Stop = 2,
        }
    }
}