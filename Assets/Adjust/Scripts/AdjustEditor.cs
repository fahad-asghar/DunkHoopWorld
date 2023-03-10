using System;
using System.Collections;
using System.Collections.Generic;
using AdjustNS.MiniJSON;
using UnityEngine;

namespace AdjustNS
{
    public class AdjustEditor : AdjustBase
    {
        static AdjustEditor()
        {
#if UNITY_EDITOR
            InitCallbackManager();
#endif
        }

        private IEnumerator SecondLayerCoroutine(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action.Invoke();
        }

        private void StartTimer(float time, Action action)
        {
            AdjustCallbackManager.Instance.StartCoroutine(SecondLayerCoroutine(time, action));
        }


        public override void Init(Action<InitSuccessResult> success, Action<InitFailedResult> failed)
        {
            if (success != null)
            {
                success(new InitSuccessResult());
            }

            StartTimer(1.5f, () =>
            {
                if (AdjustCallbackManager.AdjustClientListener != null)
                {
                    AdjustCallbackManager.AdjustClientListener.onStuffTurnChanged(false);
                }
            });
            StartTimer(8.0f, () =>
            {
                if (AdjustCallbackManager.AdjustClientListener != null)
                {
                    // 模拟动态刷新
                    AdjustCallbackManager.AdjustClientListener.onStuffTurnChanged(true);
                }
            });
            
            LogVersion();
        }

        public override void SetClientListener(AdjustClientListener listener)
        {
            AdjustCallbackManager.AdjustClientListener = listener;
        }

        public override void SetBannerAdListener(AdjustBannerAdListener listener)
        {
            AdjustCallbackManager.AdjustBannerAdListener = listener;
        }

        public override void ShowBannerAd(BannerADPosition position)
        {
            Debug.Log("ShowBannerAd");
            if (AdjustCallbackManager.AdjustBannerAdListener != null)
            {
                AdjustCallbackManager.AdjustBannerAdListener.onBannerShow("editor");
                AdjustCallbackManager.AdjustBannerAdListener.onBannerClicked("editor");
            }
        }

        public override void DismissBannerAd()
        {
            Debug.Log("ShowBannerAd");
            if (AdjustCallbackManager.AdjustBannerAdListener != null)
            {
                AdjustCallbackManager.AdjustBannerAdListener.onBannerClose("editor");
            }
        }

        public override void SetInterstitialAdListener(AdjustInterstitialAdListener listener)
        {
            AdjustCallbackManager.AdjustInterstitialAdListener = listener;
        }

        public override bool HasInterstitialAd(string gameEntry)
        {
            return true;
        }

        public override void ShowInterstitialAd(string gameEntry)
        {
            Debug.Log("ShowInterstitialAd");

            if (AdjustCallbackManager.AdjustInterstitialAdListener != null)
            {
                AdjustCallbackManager.AdjustInterstitialAdListener.onInterstitialAdShow(gameEntry);
                AdjustCallbackManager.AdjustInterstitialAdListener.onInterstitialAdClicked(gameEntry);
                AdjustCallbackManager.AdjustInterstitialAdListener.onInterstitialAdClose(gameEntry);
            }
        }

        public override void SetRewardedVideoAdListener(AdjustRewardedVideoAdListener listener)
        {
            AdjustCallbackManager.AdjustRewardedVideoAdListener = listener;
        }

        /// <summary>
        /// return rewarded video is ready
        /// </summary>
        /// <param name="gameEntry">videoEntry entry</param>
        /// <returns></returns>
        public override bool HasRewardedVideoAd(string gameEntry)
        {
            return true;
        }

        public override void ShowRewardedVideoAd(string gameEntry)
        {
            Debug.Log("ShowRewardVideoAd");
            if (AdjustCallbackManager.AdjustRewardedVideoAdListener != null)
            {
                AdjustCallbackManager.AdjustRewardedVideoAdListener.onRewardedVideoAdPlayStart(gameEntry);
                AdjustCallbackManager.AdjustRewardedVideoAdListener.onRewardedVideoAdPlayClicked(gameEntry);
                AdjustCallbackManager.AdjustRewardedVideoAdListener.onReward(gameEntry);
                AdjustCallbackManager.AdjustRewardedVideoAdListener.onRewardedVideoAdClosed(gameEntry);
            }
        }

        public override void LogEvent(string name)
        {
            Debug.Log("LogEvent name " + name);
        }

        public override void LogEventStatus(string name, string value)
        {
            Debug.Log($"LogEventStatus name: {name} value: {value}");
        }

        public override void LogEventNormal(string name, Dictionary<string, string> data)
        {
            Debug.Log($"LogEventNormal name: {name} value: {Json.Serialize(data)}");
        }



        public override void CollectEmail(string email)
        {
            Debug.Log("CollectEmail email " + email);
        }

        public override void ReportError(string msg, string title)
        {
            Debug.Log($"ReportError msg: {msg} title: {title}");
        }

        public override void ToStore()
        {
            Debug.Log("toStore");
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
    }
}