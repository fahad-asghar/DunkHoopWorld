using UnityEngine;

namespace AdjustNS
{
    public interface AdjustInterstitialAdListener
    {
        void onInterstitialAdShow(string gameEntry);

        void onInterstitialAdClicked(string gameEntry);

        void onInterstitialAdClose(string gameEntry);
    }
}