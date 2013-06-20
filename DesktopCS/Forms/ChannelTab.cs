using System;

namespace DesktopCS.Forms
{
    [System.ComponentModel.DesignerCategory("")]
    public class ChannelTab : BaseTab
    {
        public NetIRC.Channel Channel;

        public ChannelTab(NetIRC.Channel channel) : base("#" + channel.Name)
        {
            this.Channel = channel;

            this.Type = TabType.Channel;
        }
    }
}
