using UnityEngine;

namespace AdjustNS
{
    public interface AdjustRewardedVideoAdListener
    {
        void onRewardedVideoAdPlayStart(string gameEntry);

        void onRewardedVideoAdClosed(string gameEntry);

        void onRewardedVideoAdPlayClicked(string gameEntry);

        void onReward(string gameEntry);
    }
}