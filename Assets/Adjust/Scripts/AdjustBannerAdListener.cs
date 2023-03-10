using UnityEngine;

namespace AdjustNS
{
    public interface AdjustBannerAdListener
    {
        void onBannerShow(string gameEntry);

        void onBannerClicked(string gameEntry);

        void onBannerClose(string gameEntry);
    }
}