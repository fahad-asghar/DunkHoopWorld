using System.Collections.Generic;
using AdjustNS;

namespace AdjustDemo
{
    public class SDKDoc
    {
        public void Doc()
        {
            // init
            AdjustSDK.GetInstance().Init(
                delegate(InitSuccessResult result)
                {
                    // todo init success
                },
                delegate(InitFailedResult result)
                {
                    // todo init failed
                }
            );

            // sdk listener
            AdjustSDK.GetInstance().SetClientListener(new DemoAdjustClientListener());
            
            // ad listener
            AdjustSDK.GetInstance().SetBannerAdListener(new DemoAdjustBannerAdListener());
            AdjustSDK.GetInstance().SetInterstitialAdListener(new DemoAdjustInterstitialAdListener());
            AdjustSDK.GetInstance().SetRewardedVideoAdListener(new DemoAdjustRewardedVideoAdListener());

            // banner ad
            AdjustSDK.GetInstance().ShowBannerAd(BannerADPosition.bottomCenter);
            AdjustSDK.GetInstance().DismissBannerAd();

            // interstitial ad
            AdjustSDK.GetInstance().HasInterstitialAd("game");
            AdjustSDK.GetInstance().ShowInterstitialAd("game");
            
            // video ad
            AdjustSDK.GetInstance().HasRewardedVideoAd("game");
            AdjustSDK.GetInstance().ShowRewardedVideoAd("game");
            
            // log
            AdjustSDK.GetInstance().LogEvent("eventName");
            AdjustSDK.GetInstance().LogEventStatus("eventName", "value");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["aaa"] = "bbb";
            dic["ccc"] = "ddd";
            AdjustSDK.GetInstance().LogEventNormal("eventName", dic);


            // 客户端开启兑换功能(若onStuffTurnChanged回调的isOpen=true调用) （Paypal_client_open 事件）
            AdjustSDKUtils.GetInstance().PpClientOpen();

            // 主页展示Pp卡，SDK通知兑换功能开启后，游戏开启成功时上报 （Paypal_homepage 事件）
            AdjustSDKUtils.GetInstance().PpHomePageImpression();

            // Pp卡奖励页 展示，带获取奖励（请求广告）的按钮的弹窗展示时上报，包含签到页面 （PayPal_impression 事件）
            AdjustSDKUtils.GetInstance().PpRewardPageImpression();

        }

        // sdk listener
        private class DemoAdjustClientListener : AdjustClientListener
        {
            public void abVersion(string version)
            { 
            }

            public void onStuffTurnChanged(bool isOpen)
            {
                // called when StuffTurn changed
            }

            public void paidSuccess(string info)
            {
            }

            public void paidCancel()
            {
            }

            public void paidError(string error)
            {
            }

            // 支付后但没有发放商品的订单列表
            public void incompleteList(string info)
            {
            }

            public void subsAvailable(bool available)
            {
            }

            public void startTime(string time)
            {
            }
        }

        // banner listener
        private class DemoAdjustBannerAdListener : AdjustBannerAdListener
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

        // Interstitial listener
        private class DemoAdjustInterstitialAdListener : AdjustInterstitialAdListener
        {
            public void onInterstitialAdShow(string gameEntry)
            {
                // The interstitial has been shown. Pause / save state accordingly.
                // 必须在这里暂停游戏声音播放，否则会与广告声音冲突
            }

            public void onInterstitialAdClicked(string gameEntry)
            {
                // The interstitial has been clicked. 
            }

            public void onInterstitialAdClose(string gameEntry)
            {
                // The interstitial has being dismissed. Resume / load state accordingly.
                // 在这里恢复游戏声音播放
            }
        }

        // rewarded video listener
        private class DemoAdjustRewardedVideoAdListener : AdjustRewardedVideoAdListener
        {
            public void onRewardedVideoAdPlayStart(string gameEntry)
            {
                // Called when a rewarded video starts playing.
                // 必须在这里暂停游戏声音播放，否则会与广告声音冲突
            }

            public void onRewardedVideoAdClosed(string gameEntry)
            {
                // Called when a rewarded video is closed. At this point your application should resume.
                // 在这里恢复游戏声音播放
            }

            public void onRewardedVideoAdPlayClicked(string gameEntry)
            {
                // Called when a rewarded video is clicked.
            }

            public void onReward(string gameEntry)
            {
                // Called when a rewarded video is completed and the user should be rewarded.
            }
        }
    }
}