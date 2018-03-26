using System;

namespace WPFScadaDashboard.DashboardUserControls
{
    public class DashBoardEventArgs : EventArgs
    {
        public string SenderName_ { get; set; } = "Unknown Name";

        public string MessageType_ { get; set; }

        public string MessageInfo_ { get; set; }

        public object MessageObject_ { get; set; } = null;

        public DashBoardEventArgs(string messageType_, string messageInfo_)
        {
            MessageType_ = messageType_;
            MessageInfo_ = messageInfo_;            
        }

        public DashBoardEventArgs(string messageType_, string messageInfo_, string senderName_) : this(messageType_, messageInfo_)
        {
            SenderName_ = senderName_;
        }

        public DashBoardEventArgs(string messageType_, string messageInfo_, string senderName_, object messageObject_) : this(messageType_, messageInfo_, senderName_)
        {
            MessageObject_ = messageObject_;
        }
    }
}