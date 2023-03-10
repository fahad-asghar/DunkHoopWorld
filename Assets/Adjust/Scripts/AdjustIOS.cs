using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AdjustNS.MiniJSON;
using UnityEngine;

namespace AdjustNS
{
#if UNITY_IOS && !UNITY_EDITOR
    
    public class AdjustIOS : AdjustBase
    {

        static AdjustIOS()
        {
            Debug.Log("static AdjustIOS");
#if UNITY_IOS && !UNITY_EDITOR
            InitCallbackManager();
            Debug.Log("static AdjustIOS success");
#endif
        }

        public override void Init(Action<InitSuccessResult> success, Action<InitFailedResult> failed)
        {
            AdjustCallbackManager.InitSuccessResultDelegate = success;
            AdjustCallbackManager.InitFailedResultDelegate = failed;

            Debug.Log("[AdjustAdjustIOS ] _init -1 -2");
            startInit();
            Debug.Log("[AdjustAdjustIOS ] _init -3");
            
            LogVersion();
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
            int pst = 1;
            if (position == BannerADPosition.bottomCenter)
            {
                pst = 1;
            }else if (position == BannerADPosition.topCenter)
            {
                pst = 2;
            }
            showBannerAd(pst);
        }

        public override void DismissBannerAd()
        {
            dismissBannerAd();
        }

        public override void SetInterstitialAdListener(AdjustInterstitialAdListener listener)
        {
            AdjustCallbackManager.AdjustInterstitialAdListener = listener;
        }

        public override bool HasInterstitialAd(string gameEntry)
        {
            return hasInterstitialAd(gameEntry);
        }

        public override void ShowInterstitialAd(string gameEntry)
        {
            showInterstitialAd(gameEntry);
        }

        public override void SetRewardedVideoAdListener(AdjustRewardedVideoAdListener listener)
        {
            AdjustCallbackManager.AdjustRewardedVideoAdListener = listener;
        }

        public override bool HasRewardedVideoAd(string gameEntry)
        {
            return hasRewardedVideoAd(gameEntry);
        }

        public override void ShowRewardedVideoAd(string gameEntry)
        {
            showRewardedVideoAd(gameEntry);
        }

        public override void LogEvent(string name)
        {
            logEvent(name);
        }

        public override void LogEventStatus(string name, string value)
        {
            logEventStatus(name, value);
        }

        public override void LogEventNormal(string name, Dictionary<string, string> data)
        {
             logEventNormal(name, Json.Serialize(data));
        }

        public override void CollectEmail(string email)
        {
        }

        public override void ReportError(string msg, string title)
        {
            reportError(msg, title);
        }

        public override void ToStore()
        {
            toStore();
        }

        public override void DoPay(string info)
        {
            Debug.Log("DoPay::" + info);
        }

        public override void DoSub(string info)
        {
            Debug.Log("DoSub::" + info);
        }

        public override bool SubsAvailable()
        {
            Debug.Log("SubsAvailable");
            return true;
        }

        public override void PurchaseComplete(string info)
        {
            Debug.Log("PurchaseComplete::" + info);
        }

        public override void QueryIncomplete()
        {
            Debug.Log("QueryIncomplete");
        }


    #region dllimport
        [DllImport("__Internal")]
        public static extern void startInit();
        
        [DllImport("__Internal")]
        public static extern void showBannerAd(int position);
        
        [DllImport("__Internal")]
        public static extern void dismissBannerAd();
        
        [DllImport("__Internal")]
        public static extern bool hasInterstitialAd(string gameEntry);
        
        [DllImport("__Internal")]
        public static extern void showInterstitialAd(string gameEntry);
        
        [DllImport("__Internal")]
        public static extern bool hasRewardedVideoAd(string gameEntry);
        
        [DllImport("__Internal")]
        public static extern void showRewardedVideoAd(string gameEntry);
        
        [DllImport("__Internal")]
        public static extern void logEvent(string name);
        
        [DllImport("__Internal")]
        public static extern void logEventStatus(string name, string value);
        
        [DllImport("__Internal")]
        public static extern void logEventNormal(string name, string value);
        
        [DllImport("__Internal")]
        public static extern void reportError(string msg, string title);
        
        [DllImport("__Internal")]
        public static extern void toStore();
        
    #endregion
    }
#endif
}