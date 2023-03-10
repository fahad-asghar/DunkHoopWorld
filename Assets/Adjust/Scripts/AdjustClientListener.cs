using System.Collections.Generic;

namespace AdjustNS
{
    public interface AdjustClientListener
    {
        /// <summary>
        /// redeem the switch
        /// </summary>
        /// <param name="isOpen">true is open either or not</param>
        void onStuffTurnChanged(bool isOpen);
        void abVersion(string version);

        void paidSuccess(string info);
        void paidCancel();
        void paidError(string error);
        void incompleteList(string info);
        void subsAvailable(bool available);
        void startTime(string time);
    }
}