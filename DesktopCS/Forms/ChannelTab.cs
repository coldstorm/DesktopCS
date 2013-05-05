using System;

namespace DesktopCS.Forms
{
    class ChannelTab : BaseTab
    {
        private NetIRC.Channel Channel;

        public ChannelTab(NetIRC.Channel channel) : base("#" + channel.Name)
        {
            this.Channel = channel;

            this.Type = TabType.Channel;
        }
    }
}
