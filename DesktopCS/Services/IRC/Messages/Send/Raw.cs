using System.IO;
using NetIRC;
using NetIRC.Messages;

namespace DesktopCS.Services.IRC.Messages.Send
{
    class Raw : ISendMessage
    {
        private readonly string _text;

        public Raw(string text)
        {
            this._text = text;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(this._text);
        }
    }
}
