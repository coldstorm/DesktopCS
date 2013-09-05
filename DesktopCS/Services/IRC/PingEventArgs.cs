using System;

namespace DesktopCS.Services.IRC
{
    public class PingEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public string Target { get; private set; }

        public PingEventArgs(string target)
        {
            this.Target = target;
        }
    }
}
