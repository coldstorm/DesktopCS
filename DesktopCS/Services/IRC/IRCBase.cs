using System;
using DesktopCS.Models;
using DesktopCS.MVVM;

namespace DesktopCS.Services.IRC
{
    public class IRCBase : UIInvoker, IDisposable
    {
        private readonly IRCClient _ircClient;
        private readonly Tab _tab;

        public IRCBase(IRCClient ircClient, Tab tab)
        {
            this._ircClient = ircClient;
            this._ircClient.Ping += this._ircClient_Ping;

            this._tab = tab;
        }

        private void _ircClient_Ping(object sender, PingEventArgs e)
        {
            if (e.Handled == false)
                if (e.Target == this._tab.Header)
                        e.Handled = true;
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            this._ircClient.Ping -= this._ircClient_Ping;
        }

        #endregion
    }
}
