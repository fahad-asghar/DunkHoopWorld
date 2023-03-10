using System;
using System.Collections;
using System.Collections.Generic;
using AdjustNS;
using AdjustNS.MiniJSON;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace AdjustDemo
{
    public class SDKDemoUI : MonoBehaviour
    {
        public static SDKDemoUI INSTANCE;
        public static string GAME_ENTRY = "game";

        public Text logText;
        public ToastTools toastTools;

        public Button btnClearLog;
        public Button btnToStore;
        public Button btnShowBanner;
        public Button btnHideBanner;
        public Button btnHasInterstitial;
        public Button btnShowInterstitial;
        public Button btnHashVideo;
        public Button btnShowVideo;
        public Button btnLogEvent;
        public Button btnLogEventStatus;
        public Button btnLogEventNormal;
        public Button btnSubsAvailable;
        public Button btnPay;
        public Button btnSub;


        private void Awake()
        {
            INSTANCE = this;
            // sdk init
            AdjustSDK.GetInstance()
                .Init(delegate(InitSuccessResult result) { LogPrint("init success"); },
                    delegate(InitFailedResult result) { LogPrint($"init failed msg: {result.message}"); });

            // init listener
            AdjustSDK.GetInstance().SetClientListener(new MyAdjustClientListener());
            AdjustSDK.GetInstance().SetBannerAdListener(new MyAdjustBannerAdListener());
            AdjustSDK.GetInstance().SetInterstitialAdListener(new MyAdjustInterstitialAdListener());
            AdjustSDK.GetInstance().SetRewardedVideoAdListener(new MyAdjustRewardedVideoAdListener());
        }

        public void LogPrint(string s)
        {
            var rectTransform = logText.rectTransform;
            var size = rectTransform.rect;
            rectTransform.sizeDelta = new Vector2(size.width, size.height + 22);

            logText.text = logText.text + "\n" + s;
        }

        private void Start()
        {
            this.btnClearLog.onClick.AddListener(this.OnBtnClearLogClick);
            this.btnShowBanner.onClick.AddListener(this.OnBtnShowBannerClick);
            this.btnHideBanner.onClick.AddListener(this.OnBtnHideBannerClick);
            this.btnHasInterstitial.onClick.AddListener(this.OnBtnHasInterstitialClick);
            this.btnShowInterstitial.onClick.AddListener(this.OnBtnShowInterstitialClick);
            this.btnHashVideo.onClick.AddListener(this.OnBtnHashVideoClick);
            this.btnShowVideo.onClick.AddListener(this.OnBtnShowVideoClick);
            this.btnLogEvent.onClick.AddListener(this.OnBtnLogEventClick);
            this.btnLogEventStatus.onClick.AddListener(this.OnBtnLogEventStatusClick);
            this.btnLogEventNormal.onClick.AddListener(this.OnBtnLogEventNormalClick);
            this.btnToStore.onClick.AddListener(this.OnBtnToStoreClick);
            this.btnPay.onClick.AddListener(this.OnBtnPayClick);
            this.btnSub.onClick.AddListener(this.OnBtnSubClick);
            this.btnSubsAvailable.onClick.AddListener(this.OnBtnSubsAvailableClick);
        }

        private void OnBtnClearLogClick()
        {
            this.logText.text = "";
            
            var rectTransform = logText.rectTransform;
            var size = rectTransform.rect;
            rectTransform.sizeDelta = new Vector2(size.width, 100);
        }

        private void OnBtnToStoreClick()
        {
            LogPrint("to store");
            AdjustSDK.GetInstance().ToStore();
        }

        private void OnBtnShowBannerClick()
        {
            Toast("show Banner");
            LogPrint("show banner");
            AdjustSDK.GetInstance().ShowBannerAd(BannerADPosition.bottomCenter);
        }

        private void OnBtnHideBannerClick()
        {
            Toast("hide Banner");
            LogPrint("hide banner");
            AdjustSDK.GetInstance().DismissBannerAd();
        }

        private void OnBtnHasInterstitialClick()
        {
            var isReady = AdjustSDK.GetInstance().HasInterstitialAd(GAME_ENTRY);
            LogPrint("HasInterstitialAd + " + isReady);
            Toast("" + isReady);
        }

        private void OnBtnShowInterstitialClick()
        {
            Toast("show InterstitialAd");
            LogPrint("show InterstitialAd");
            AdjustSDK.GetInstance().ShowInterstitialAd(GAME_ENTRY);
        }

        private void OnBtnHashVideoClick()
        {
            var isReady = AdjustSDK.GetInstance().HasRewardedVideoAd(GAME_ENTRY);
            LogPrint("Has Video + " + isReady);
            Toast("" + isReady);
        }

        private void OnBtnShowVideoClick()
        {
            Toast("show video");
            LogPrint("showRewardedVideoAd");
            AdjustSDK.GetInstance().ShowRewardedVideoAd(GAME_ENTRY);
        }

        private void OnBtnLogEventClick()
        {
            Toast("log event");
            AdjustSDK.GetInstance().LogEvent("adujust_sdk_test");
            LogPrint($"LogEvent name: adujust_sdk_test");
        }

        private void OnBtnLogEventStatusClick()
        {
            Toast("log status");
            AdjustSDK.GetInstance().LogEventStatus("adujust_sdk_test", "test");
            LogPrint($"LogEventStatus name: adujust_sdk_test value: test");
        }

        private void OnBtnLogEventNormalClick()
        {
            Toast("log event normal");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["aaa"] = "bbb";
            dic["ccc"] = "ddd";
            string jsonData = Json.Serialize(dic);
            AdjustSDK.GetInstance().LogEventNormal("adujust_sdk_test", dic);
            LogPrint($"logEventNormal name: adujust_sdk_test value: ${jsonData}");
        }

        private void OnBtnPayClick()
        {
            Toast("pay");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["scene"] = "1";
            dic["product"] = "iap_101";
            string jsonData = Json.Serialize(dic);
            AdjustSDK.GetInstance().DoPay(jsonData);
            LogPrint("OnBtnPayClick info:" + jsonData);
        }

        private void OnBtnSubClick()
        {
            Toast("subs");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["scene"] = "1";
            dic["product"] = "vip_101";
            string jsonData = Json.Serialize(dic);
            AdjustSDK.GetInstance().DoSub(jsonData);
            LogPrint("OnBtnSubClick info:" + jsonData);
        }

        private void OnBtnSubsAvailableClick()
        {
            var available = AdjustSDK.GetInstance().SubsAvailable();
            LogPrint("SubsAvailable + " + available);
            Toast("" + available);
        }

        private void Toast(string str)
        {
            toastTools.Toast(str);
        }

        private class MyAdjustClientListener : AdjustClientListener
        {
            public void onStuffTurnChanged(bool isOpen)
            {
                if (isOpen) {
                    AdjustSDKUtils.GetInstance().PpClientOpen();
                    SDKDemoUI.INSTANCE.LogPrint("LogEvent PpClientOpen");
                    AdjustSDKUtils.GetInstance().PpHomePageImpression();
                    SDKDemoUI.INSTANCE.LogPrint("LogEvent PpHomePageImpression");
                }
                SDKDemoUI.INSTANCE.LogPrint("[Listener] onStuffTurnChanged: " + isOpen);
            }

            public void abVersion(string version)
            {
                // a或者b
                SDKDemoUI.INSTANCE.LogPrint("abVersion::" + version);
            }

            public void paidSuccess(string info)
            {
                // info的为json数组，格式是[{"token":"xxxx","product":"test1","order_type":"inapp"}]，
                // 其中product是商品id，其余参数收到支付成功回调后遍历发放道具
                SDKDemoUI.INSTANCE.LogPrint("[Listener] paidSuccess: " + info);
                // 发放购买的商品后调用该方法告诉sdk订单完成
                AdjustSDK.GetInstance().PurchaseComplete(info);
            }

            public void paidCancel()
            {
                SDKDemoUI.INSTANCE.LogPrint("[Listener] paidCancel");
            }

            public void paidError(string error)
            {
                SDKDemoUI.INSTANCE.LogPrint("[Listener] paidError: " + error);
            }

            // 支付后但没有发放商品的订单列表
            public void incompleteList(string info)
            {
                // info的为json数组，格式是[{"token":"xxxx","product":"test1","order_type":"inapp"}]，
                // 其中product是商品id，其余参数收到支付成功回调后遍历发放道具
                SDKDemoUI.INSTANCE.LogPrint("[Listener] incompleteList: " + info);
                // 发放商品后调用该方法告诉sdk订单完成
                AdjustSDK.GetInstance().PurchaseComplete(info);
            }

            public void subsAvailable(bool available)
            {
                SDKDemoUI.INSTANCE.LogPrint("[Listener] subsAvailable: " + available);
            }

            public void startTime(string time)
            {
            }
        }

        private class MyAdjustBannerAdListener : AdjustBannerAdListener
        {
            public void onBannerShow(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[BannerAdListener] onBannerShow: " + gameEntry);
            }

            public void onBannerClicked(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[BannerAdListener] onBannerClicked: " + gameEntry);
            }

            public void onBannerClose(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[BannerAdListener] onBannerClose: " + gameEntry);
            }
        }

        private class MyAdjustInterstitialAdListener : AdjustInterstitialAdListener
        {
            public void onInterstitialAdShow(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[InterstitialAdListener] onInterstitialAdShow: " + gameEntry);
            }

            public void onInterstitialAdClicked(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[InterstitialAdListener] onInterstitialAdClicked: " + gameEntry);
            }

            public void onInterstitialAdClose(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[InterstitialAdListener] onInterstitialAdClose: " + gameEntry);
            }
        }

        private class MyAdjustRewardedVideoAdListener : AdjustRewardedVideoAdListener
        {
            public void onRewardedVideoAdPlayStart(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[RewardedVideoAdListener] onRewardedVideoAdPlayStart: " + gameEntry);
            }

            public void onRewardedVideoAdClosed(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[RewardedVideoAdListener] onRewardedVideoAdClosed: " + gameEntry);
            }

            public void onRewardedVideoAdPlayClicked(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[RewardedVideoAdListener] onRewardedVideoAdPlayClicked: " + gameEntry);
            }

            public void onReward(string gameEntry)
            {
                SDKDemoUI.INSTANCE.LogPrint("[RewardedVideoAdListener] onReward: " + gameEntry);
            }
        }
    }
}