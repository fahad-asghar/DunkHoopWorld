using System;
using System.Collections.Generic;
using AdjustNS.MiniJSON;
using UnityEngine;

namespace AdjustNS
{
    public class AdjustCallbackManager : MonoBehaviour
    {
        private static string TAG = "[AdjustCallbackManager] ";
        public static AdjustCallbackManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            Application.logMessageReceivedThreaded += OnLogReceivedCallback;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
            
            Application.logMessageReceivedThreaded -= OnLogReceivedCallback;
        }

        private void OnLogReceivedCallback(string condition, string stackTrace, LogType type)
        {
            if (!AdjustSDK.IsCreated())
            {
                return;
            }
            // 上报
            string currLog = condition;
            if (type == LogType.Exception)
            {
                currLog += ("\n" + stackTrace + "|->EndReportError");
                AdjustSDK.GetInstance().ReportError("EXCEPTION: " + currLog, "EXCEPTION");
            }
            else if (type == LogType.Error)
            {
                currLog += ("\n" + stackTrace + "|->EndReportError");
                AdjustSDK.GetInstance().ReportError("ERROR: " + currLog, "ERROR");
            }
        }

        private void SdkLog(string s)
        {
            Debug.Log(s);
        }

        // region data
        private static ClientData ClientData = new ClientData();
        // endregion

        #region native callback action

        public static Action<InitSuccessResult> InitSuccessResultDelegate = null;
        public static Action<InitFailedResult> InitFailedResultDelegate = null;
        public static AdjustClientListener AdjustClientListener = null;
        public static AdjustBannerAdListener AdjustBannerAdListener = null;
        public static AdjustInterstitialAdListener AdjustInterstitialAdListener = null;
        public static AdjustRewardedVideoAdListener AdjustRewardedVideoAdListener = null;

        public static void SetAdjustClientListener(AdjustClientListener listener)
        {
            AdjustClientListener = listener;
            if (AdjustClientListener != null)
            {
                if (ClientData.needCallStuffTurn)
                {
                    AdjustClientListener.onStuffTurnChanged(ClientData.stuffTurn);
                }
            }
        }

        #endregion


        #region native callback method

        public void onPluginInitSuccess(string args)
        {
            SdkLog($"{TAG} onPluginInitSuccess...{args}");
            if (InitSuccessResultDelegate != null)
            {
                InitSuccessResultDelegate(new InitSuccessResult());
            }
        }

        public void onPluginInitFailed(string args)
        {
            SdkLog($"{TAG} onPluginInitFailed...{args}");
            if (InitFailedResultDelegate != null)
            {
                InitFailedResultDelegate(new InitFailedResult(100, args));
            }
        }

        public void onStuffTurnChanged(string args)
        {
            SdkLog($"{TAG} onStuffTurnChanged...{args}");
            bool isOpen = (args.ToLower() == "true");
            ClientData.stuffTurn = isOpen;

            if (AdjustClientListener != null)
            {
                AdjustClientListener.onStuffTurnChanged(isOpen);
                ClientData.needCallStuffTurn = false;
            }
            else
            {
                ClientData.needCallStuffTurn = true;
            }

            AdjustSDK.GetInstance().LogEventStatus("PayPal_turn_on", isOpen ? "1" : "0");
        }


        public void abVersion(string args)
        {
            SdkLog($"{TAG} adVersion...{args}");

            if (AdjustClientListener != null)
            {
                AdjustClientListener.abVersion(args);
            }
        }

        public void paidSuccess(string args)
        {
            SdkLog($"{TAG} paidSuccess...{args}");
            if (AdjustClientListener != null)
            {
                AdjustClientListener.paidSuccess(args);
            }
        }

        public void paidCancel(string args)
        {
            SdkLog($"{TAG} paidCancel...{args}");
            if (AdjustClientListener != null)
            {
                AdjustClientListener.paidCancel();
            }
        }

        public void paidError(string args)
        {
            SdkLog($"{TAG} paidError...{args}");
            if (AdjustClientListener != null)
            {
                AdjustClientListener.paidError(args);
            }
        }

        public void incompleteList(string args)
        {
            SdkLog($"{TAG} incompleteList...{args}");
            if (AdjustClientListener != null)
            {
                AdjustClientListener.incompleteList(args);
            }
        }

        public void subsAvailable(string args)
        {
            SdkLog($"{TAG} subsAvailable...{args}");
            bool available = (args.ToLower() == "true");

            if (AdjustClientListener != null)
            {
                AdjustClientListener.subsAvailable(available);
            }
        }


        public void startTime(string time)
        {
            SdkLog($"{TAG} startTime...{time}");
            if (AdjustClientListener != null)
            {
                AdjustClientListener.startTime(time);
            }
        }

        public void onBannerShow(string args)
        {
            SdkLog($"{TAG} onBannerShow...{args}");
            if (AdjustBannerAdListener != null)
            {
                AdjustBannerAdListener.onBannerShow(args);
            }
        }

        public void onBannerClicked(string args)
        {
            SdkLog($"{TAG} onBannerClicked...{args}");
            if (AdjustBannerAdListener != null)
            {
                AdjustBannerAdListener.onBannerClicked(args);
            }
        }

        public void onBannerClose(string args)
        {
            SdkLog($"{TAG} onBannerClose...{args}");
            if (AdjustBannerAdListener != null)
            {
                AdjustBannerAdListener.onBannerClose(args);
            }
        }

        public void onInterstitialAdShow(string args)
        {
            SdkLog($"{TAG} onInterstitialAdShow...{args}");
            if (AdjustInterstitialAdListener != null)
            {
                AdjustInterstitialAdListener.onInterstitialAdShow(args);
            }
        }

        public void onInterstitialAdClicked(string args)
        {
            SdkLog($"{TAG} onInterstitialAdClicked...{args}");
            if (AdjustInterstitialAdListener != null)
            {
                AdjustInterstitialAdListener.onInterstitialAdClicked(args);
            }
        }

        public void onInterstitialAdClose(string args)
        {
            SdkLog($"{TAG} onInterstitialAdClose...{args}");
            if (AdjustInterstitialAdListener != null)
            {
                AdjustInterstitialAdListener.onInterstitialAdClose(args);
            }
        }

        public void onRewardedVideoAdPlayStart(string args)
        {
            SdkLog($"{TAG} onRewardedVideoAdPlayStart...{args}");
            if (AdjustRewardedVideoAdListener != null)
            {
                AdjustRewardedVideoAdListener.onRewardedVideoAdPlayStart(args);
            }
        }

        public void onRewardedVideoAdPlayClicked(string args)
        {
            SdkLog($"{TAG} onRewardedVideoAdPlayClicked...{args}");
            if (AdjustRewardedVideoAdListener != null)
            {
                AdjustRewardedVideoAdListener.onRewardedVideoAdPlayClicked(args);
            }
        }

        public void onReward(string args)
        {
            SdkLog($"{TAG} onReward...{args}");
            if (AdjustRewardedVideoAdListener != null)
            {
                AdjustRewardedVideoAdListener.onReward(args);
            }
        }

        public void onRewardedVideoAdClosed(string args)
        {
            SdkLog($"{TAG} onRewardedVideoAdClosed...{args}");
            if (AdjustRewardedVideoAdListener != null)
            {
                AdjustRewardedVideoAdListener.onRewardedVideoAdClosed(args);
            }
        }

        #endregion
    }

    class ClientData
    {
        public bool stuffTurn = false;
        public string inviteCode = "";

        public bool needCallStuffTurn = false;
        public bool needCallInviteCode = false;
    }
}