using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AdjustNS
{
    public abstract class AdjustBase
    {
        protected static string PLUGIN_SDK_VERSION = "1.0.8";

        protected static void InitCallbackManager()
        {
            Debug.Log("InitCallbackManager");
            Debug.Log("PluginSDK-version-" + PLUGIN_SDK_VERSION);
            var type = typeof(AdjustCallbackManager);
            var mgr = new GameObject("AdjustCallbackManager", type).GetComponent<AdjustCallbackManager>();
            // Its Awake() method sets Instance.
            if (AdjustCallbackManager.Instance != mgr)
            {
                Debug.LogWarning("It looks like you have the " + type.Name +
                                 " on a GameObject in your scene. Please remove the script from your scene.");
            }
        }

        protected void LogVersion()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["name"] = "cp_unity_sdk";
            data["value"] = PLUGIN_SDK_VERSION;
            LogEventNormal("newbyear_lib_ver", data);

            Dictionary<string, string> engineData = new Dictionary<string, string>();
            engineData["name"] = "engine_unity";
            engineData["value"] = Application.unityVersion;
            LogEventNormal("newbyear_lib_ver", engineData);
            
        }

        public abstract void Init(Action<InitSuccessResult> success, Action<InitFailedResult> failed);

        public abstract void SetClientListener(AdjustClientListener listener);


        public abstract void SetBannerAdListener(AdjustBannerAdListener listener);
        public abstract void ShowBannerAd(BannerADPosition position);
        public abstract void DismissBannerAd();

        public abstract void SetInterstitialAdListener(AdjustInterstitialAdListener listener);
        public abstract bool HasInterstitialAd(string gameEntry);
        public abstract void ShowInterstitialAd(string gameEntry);

        public abstract void SetRewardedVideoAdListener(AdjustRewardedVideoAdListener listener);
        public abstract bool HasRewardedVideoAd(string gameEntry);
        public abstract void ShowRewardedVideoAd(string gameEntry);

        public abstract void LogEvent(string name);
        public abstract void LogEventStatus(string name, string value);

        public abstract void LogEventNormal(string name, Dictionary<string, string> data);
        public abstract void CollectEmail(string email);

        public abstract void ReportError(string msg, string title);

        public abstract void ToStore();

        public abstract void DoPay(string info);
        public abstract void DoSub(string info);
        public abstract bool SubsAvailable();
        public abstract void PurchaseComplete(string info);
        public abstract void QueryIncomplete();
        // public abstract void SetLogDebug(bool isDebug);
    }
}