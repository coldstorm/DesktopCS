using System.Collections.Generic;
using System.IO;
using NetIRC;
using NetIRC.Messages;

namespace DesktopCS.Services.IRC.Messages.Send
{
    class SendCollection : List<ISendMessage>, ISendMessage
    {
        public SendCollection()
        {
                
        }

        public SendCollection(IEnumerable<ISendMessage> collection) 
            : base(collection)
        {

        }

        #region ISendMessage Members

        public void Send(StreamWriter writer, Client client)
        {
            foreach (var message in this)
            {
                client.Send(message);
            }
        }

        #endregion
    }
}
