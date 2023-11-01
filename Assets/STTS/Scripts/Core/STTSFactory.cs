using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STTSCore.Engine
{
    public static class STTSFactory
    {
        private static STTS instance;

        public static STTS GetInstance()
        {
            if (instance == null)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    instance = new STTSAndroid();
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    instance = new STTSiOS();
                }
                else
                {
                    return null;
                }
            }
            

            return instance;
        }
    }
}