namespace AdjustNS
{
    /**
     * sdk tools
     */
    public class AdjustSDKUtils
    {
        private static AdjustSDKUtils INSTANCE = new AdjustSDKUtils();

        public static AdjustSDKUtils GetInstance()
        {
            return INSTANCE;
        }

        public void PpClientOpen()
        {
            AdjustSDK.GetInstance().LogEvent("Paypal_client_open");
        }

        public void PpHomePageImpression()
        {
            AdjustSDK.GetInstance().LogEvent("Paypal_homepage");
        }

        public void PpRewardPageImpression()
        {
            AdjustSDK.GetInstance().LogEvent("PayPal_impression");
        }
        
        public void PpRewardPageRewardBtnClick()
        {
            AdjustSDK.GetInstance().LogEvent("PayPal_click");
        }
        
        public void PpRewardPageRewardSuccess()
        {
            AdjustSDK.GetInstance().LogEvent("PayPal_reward_success");
        }
    }
}