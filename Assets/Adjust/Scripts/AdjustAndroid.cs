using System;
using System.Collections.Generic;
using AdjustNS.MiniJSON;
using UnityEngine;

namespace AdjustNS
{
    public class AdjustAndroid : AdjustBase
    {
        private static string ANDROID_CLASS_NAME = "com.adjust.android.sdk.AdjustPlugin";

        static AdjustAndroid()
        {
            Debug.Log("static AdjustAndroid");
#if UNITY_ANDROID
            InitCallbackManager();
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
            // foreach (GameObject go in allObjects)
            // {
            //     Debug.Log("AdjustAndroid is an active object::" + go);
            //     Debug.Log("AdjustAndroid is an active object::" + go.name);
            // }
            Debug.Log("static AdjustAndroid success");
#endif
        }

        public static AndroidJavaObject CreateJavaMapFromDictainary(IDictionary<string, string> parameters)
        {
            AndroidJavaObject javaMap = new AndroidJavaObject("java.util.HashMap");
            IntPtr putMethod = AndroidJNIHelper.GetMethodID(
                javaMap.GetRawClass(), "put",
                "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

            object[] args = new object[2];
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                using (AndroidJavaObject k = new AndroidJavaObject(
                    "java.lang.String", kvp.Key))
                {
                    using (AndroidJavaObject v = new AndroidJavaObject(
                        "java.lang.String", kvp.Value))
                    {
                        args[0] = k;
                        args[1] = v;
                        AndroidJNI.CallObjectMethod(javaMap.GetRawObject(),
                            putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }

            return javaMap;
        }

        public override void Init(Action<InitSuccessResult> success, Action<InitFailedResult> failed)
        {
            AdjustCallbackManager.InitSuccessResultDelegate = success;
            AdjustCallbackManager.InitFailedResultDelegate = failed;

            LogVersion();
            Debug.Log("[AdjustAndroid ] _init -1");
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                Debug.Log("[AdjustAndroid ] _init -2");
                sdkClass.CallStatic("init", PLUGIN_SDK_VERSION);
                Debug.Log("[AdjustAndroid ] _init -3");
            }
            
        }

        public override void SetClientListener(AdjustClientListener listener)
        {
            AdjustCallbackManager.SetAdjustClientListener(listener);
        }

        public override void SetBannerAdListener(AdjustBannerAdListener listener)
        {
            AdjustCallbackManager.AdjustBannerAdListener = listener;
        }

        public override void ShowBannerAd(BannerADPosition position)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("showBannerAd", (int) position);
            }
        }

        public override void DismissBannerAd()
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("dismissBannerAd");
            }
        }

        public override void SetInterstitialAdListener(AdjustInterstitialAdListener listener)
        {
            AdjustCallbackManager.AdjustInterstitialAdListener = listener;
        }

        public override bool HasInterstitialAd(string gameEntry)
        {
            bool isReady = false;
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                isReady = sdkClass.CallStatic<bool>("hasInterstitialAd", gameEntry);
            }

            return isReady;
        }

        public override void ShowInterstitialAd(string gameEntry)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("showInterstitialAd", gameEntry);
            }
        }

        public override void SetRewardedVideoAdListener(AdjustRewardedVideoAdListener listener)
        {
            AdjustCallbackManager.AdjustRewardedVideoAdListener = listener;
        }

        public override bool HasRewardedVideoAd(string gameEntry)
        {
            bool isReady = false;
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                isReady = sdkClass.CallStatic<bool>("hasRewardedVideoAd", gameEntry);
            }

            return isReady;
        }

        public override void ShowRewardedVideoAd(string gameEntry)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("showRewardedVideoAd", gameEntry);
            }
        }

        public override void LogEvent(string name)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("logEvent", name);
            }
        }

        public override void LogEventStatus(string name, string value)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("logEventStatus", name, value);
            }
        }

        public override void LogEventNormal(string name, Dictionary<string, string> data)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                string jsonData = Json.Serialize(data);
                sdkClass.CallStatic("logEventNormal", name, jsonData);
            }
        }

        public override void CollectEmail(string email)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("collectEmail", email);
            }
        }

        public override void ReportError(string msg, string title)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("reportError", msg);
            }
        }

        public override void ToStore()
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("toStore");
            }
        }

        public override void DoPay(string info)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("doPay", info);
            }
        }

        public override void DoSub(string info)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("doSub", info);
            }
        }

        public override bool SubsAvailable()
        {
            bool isReady = false;
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                isReady = sdkClass.CallStatic<bool>("getSubsAvailable");
            }

            return isReady;
        }

        public override void PurchaseComplete(string info)
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("purchaseComplete", info);
            }
        }

        public override void QueryIncomplete()
        {
            using (AndroidJavaClass sdkClass = new AndroidJavaClass(ANDROID_CLASS_NAME))
            {
                sdkClass.CallStatic("queryIncomplete");
            }
        }
    }
}