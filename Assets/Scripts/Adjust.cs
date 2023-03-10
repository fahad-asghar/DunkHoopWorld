using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdjustNS;
using AdjustDemo;
using System;

public class Adjust : MonoBehaviour
{
    public static Adjust Instance;
    [SerializeField]
    public bool isInitialized;
    [SerializeField] string rewardedItem;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AdjustSDK.GetInstance().Init(
       delegate (InitSuccessResult result)
       {
           isInitialized = true;
        // todo init success
    },
       delegate (InitFailedResult result)
       {
           isInitialized = false;
           // todo init failed
       }
   );

        AdjustSDK.GetInstance().SetClientListener(new DemoAdjustClientListener());
        AdjustSDK.GetInstance().SetRewardedVideoAdListener(new DemoAdjustRewardedVideoAdListener());
        IsRewardedAvailable();

    }
    
    public bool IsRewardedAvailable()
    {
        return AdjustSDK.GetInstance().HasRewardedVideoAd("game");
    }

    public void ShowRewarded()
    {
        AdjustSDK.GetInstance().ShowRewardedVideoAd("game");
    }

    public void ShowRewardedAd(string rewardedItem)
    {
        if (Adjust.Instance.IsRewardedAvailable())
        {
            this.rewardedItem = rewardedItem;
            Adjust.Instance.ShowRewarded();
        }
    }

    public void Reward()
    {
        if (rewardedItem.Equals("Balls"))
            GameManager.instance.BuyBasketballs(0);
        else if (rewardedItem.Equals("ClearObstacles"))
            GameManager.instance.ClearObstacles(0);
        else if (rewardedItem.Equals("EnlargeBasket"))
            GameManager.instance.EnlargeBasket(0);
        else if (rewardedItem.Equals("SlotMachine"))
            GameManager.instance.ActivateSlotMachine(0);
    }

    private class DemoAdjustClientListener : AdjustClientListener
    {
        public void abVersion(string version)
        {
            throw new System.NotImplementedException();
        }

        public void incompleteList(string info)
        {
            throw new System.NotImplementedException();
        }

        public void onBindSuccess()
        {
            throw new System.NotImplementedException();
        }

        public void onStuffTurnChanged(bool isOpen)
        {
            // called when StuffTurn changed
            if (isOpen)
            {
                // 上报客户端开启兑换功能
                AdjustSDKUtils.GetInstance().PpClientOpen();
                // 展示banner的时机
                AdjustSDK.GetInstance().ShowBannerAd(BannerADPosition.bottomCenter); 
                AdjustSDKUtils.GetInstance().PpHomePageImpression();
            }
            else
            {

            }            
            // called when StuffTurn changed
            // 兑换开关，isOpen为true表示开启，false表示未开启。
            // 这个接口会动态刷新，实时变化，需要开发者自行测试。
        }

        public void paidCancel()
        {
            throw new System.NotImplementedException();
        }

        public void paidError(string error)
        {
            throw new System.NotImplementedException();
        }

        public void paidSuccess(string info)
        {
            throw new System.NotImplementedException();
        }

        public void startTime(string time)
        {
            throw new System.NotImplementedException();
        }

        public void subsAvailable(bool available)
        {
            throw new System.NotImplementedException();
        }

        // A或B等多版本和对应配置，后台没有配置将没有该回调
        public void versionConfig(string json)
        {
            //格式 {"config_key":"A","config_value":"{}"}，解析这串json拿到version和对应的配置
            SDKDemoUI.INSTANCE.LogPrint("versionConfig::" + json);
        }

        
    }

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
            Adjust.Instance.Reward();
            // Called when a rewarded video is completed and the user should be rewarded.
        }
    }
}
