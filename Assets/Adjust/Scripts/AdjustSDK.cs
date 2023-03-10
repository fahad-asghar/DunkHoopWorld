using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdjustNS
{
    public class AdjustSDK :
#if UNITY_EDITOR
        AdjustEditor
#elif UNITY_ANDROID && !UNITY_EDITOR
        AdjustAndroid
#elif UNITY_IOS && !UNITY_EDITOR
        AdjustIOS
#else
        AdjustEditor
#endif
    {
        static private AdjustSDK Instance;

        static public AdjustSDK GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AdjustSDK();
            }

            return Instance;
        }

        public static bool IsCreated()
        {
            return Instance != null;
        }
    }


    [Serializable]
    public class InitSuccessResult
    {
    }

    [Serializable]
    public class InitFailedResult
    {
        public int code;
        public string message = "";

        public InitFailedResult(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }

    public enum BannerADPosition
    {
        bottomCenter = 1,
        topCenter = 2,
    }
}