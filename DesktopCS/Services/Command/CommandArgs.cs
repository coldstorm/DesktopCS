using System;
using DesktopCS.Models;
using NetIRC;

namespace DesktopCS.Services.Command
{
    public class CommandArgs : EventArgs
    {
        public string FullCommand { get; set; }
        public Client Client { get; set; }
        public string[] Parameters { get; set; }
        public Tab Tab { get; set; }

        public CommandArgs(string fullCommand, Client client, Tab tab, string[] parameters)
        {
            this.FullCommand = fullCommand;
            this.Client = client;
            this.Parameters = parameters;
            this.Tab = tab;
        }
    }
}
