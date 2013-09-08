using System;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.Command
{
    public class CommandArgs : EventArgs
    {
        public Client Client { get; set; }
        public string[] Parameters { get; set; }
        public Tab Tab { get; set; }

        public CommandArgs(Client client, Tab tab, string[] parameters)
        {
            this.Client = client;
            this.Parameters = parameters;
            this.Tab = tab;
        }
    }
}
