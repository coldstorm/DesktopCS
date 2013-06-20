using System;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    public class PrivateMessageTab : BaseTab
    {
        public NetIRC.User Target;

        public PrivateMessageTab(NetIRC.User target) : base(target.NickName)
        {
            this.Target = target;

            this.Type = TabType.PrivateMessage;
        }
    }
}
