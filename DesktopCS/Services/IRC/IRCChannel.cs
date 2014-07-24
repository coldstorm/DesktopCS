using System;
using System.Collections.Generic;
using DesktopCS.Helpers.Extensions;
using DesktopCS.Models;
using NetIRC;
using NetIRC.Messages.Send;
using DesktopCS.Helpers;

namespace DesktopCS.Services.IRC
{
    public class IRCChannel : IRCBase
    {
        private readonly IRCClient _ircClient;
        private readonly ChannelTab _channelTab;
        private readonly Channel _channel;

        public IRCChannel(IRCClient ircClient, ChannelTab channelTab, Channel channel) 
            : base(ircClient, channelTab)
        {
            this._ircClient = ircClient;
            this._ircClient.ChannelLeave += this._ircClient_ChannelLeave;

            this._channel = channel;
            this._channel.OnAction += this._channel_OnAction;
            this._channel.OnMessage += this._channel_OnMessage;
            this._channel.OnJoin += this._channel_OnJoin;
            this._channel.OnLeave += this._channel_OnLeave;
            this._channel.OnNames += this._channel_OnNames;
            this._channel.OnTopic += this._channel_OnTopic;
            this._channel.OnTopicChange += this._channel_OnTopicChange;
            this._channel.OnWho += this._channel_OnWho;
            this._channel.OnMode += this._channel_OnMode;
            this._channel.OnKick += this._channel_OnKick;
            this._channel.OnChannelOperatorNeeded += this._channel_OnChannelOperatorNeeded;
            this._channel.OnCannotSendToChannel += this._channel_OnCannotSendToChannel;

            this._channelTab = channelTab;
            this._channelTab.Part += this._channelTab_Part;
            this._channelTab.AddJoin(this.GetArgs());
            this._channelTab.Header = this._channel.FullName; // Correct casing
        }

        private void AddUser(Channel channel, User user)
        {
            UserItem userItem = user.ToUserItem(this._channel);
            this._channelTab.AddUser(userItem);

            new IRCChannelUser(this._ircClient, this._channelTab, userItem, user, channel);
        }

        #region Event Handlers

        private void _channelTab_Part(object sender, EventArgs e)
        {
            this._ircClient.Send(this._channel.Part());
        }

        private void _ircClient_ChannelLeave(object sender, Channel channel)
        {
            this.Run(() =>
            {
                if (channel == this._channel)
                {
                    this._channelTab.Users.Clear();
                    this.Dispose();
                }
            });
        }

        private void _channel_OnNames(Channel source, string[] users)
        {
            this.Run(() =>
            {
                this._channelTab.Users.Clear();
                foreach (var user in this._channel.Users.Values)
                {
                    this.AddUser(this._channel, user);
                }
            });
        }

        private void _channel_OnLeave(Channel source, User user, string reason)
        {
            this.Run(() =>
            {
                this._channelTab.AddLeave(user.NickName, reason, this.GetArgs());
                this._channelTab.RemoveUser(user.ToUserItem(source));
            });
        }

        private void _channel_OnJoin(Channel source, User user)
        {
            this.Run(() =>
            {
                this._channelTab.AddJoin(user.NickName, this.GetArgs());
                this.AddUser(this._channel, user);
            });
        }

        private void _channel_OnMessage(Channel source, User user, string message)
        {
            this.Run(() => this._channelTab.AddChat(user, this._channel, message, this.GetArgs()));
        }

        private void _channel_OnTopic(Channel source, ChannelTopic topic)
        {
            this.Run(() =>
            {
                this._channelTab.Topic.Author = topic.Author.ToUserItem();
                this._channelTab.Topic.Content = topic.Message;
                this._channelTab.Topic.AuthorDate = topic.LastUpdated;
                this._channelTab.AddTopic(source.FullName, topic.Author.NickName, topic.LastUpdated, this.GetArgs());
            });
        }

        private void _channel_OnTopicChange(Channel source, ChannelTopic topic)
        {
            this.Run(() =>
            {
                this._channelTab.Topic.Author = topic.Author.ToUserItem();
                this._channelTab.Topic.Content = topic.Message;
                this._channelTab.Topic.AuthorDate = topic.LastUpdated;
                this._channelTab.AddTopicChanged(topic.Author.NickName, this.GetArgs());
            });
        }

        private void _channel_OnMode(Channel source, User setter, List<KeyValuePair<string, string>> changes)
        {
            this.Run(() =>
            {
                foreach (KeyValuePair<string, string> change in changes)
                {
                    if (NetIRCHelper.ChannelRankModeChars.ContainsKey(change.Key[1]))
                    {
                        if (change.Key[0] == '+')
                        {
                            this._channelTab.AddRankGiven(setter.NickName, change.Value, NetIRCHelper.ChannelRankModeChars[change.Key[1]], this.GetArgs());
                        }

                        else
                        {
                            this._channelTab.AddRankTaken(setter.NickName, change.Value, NetIRCHelper.ChannelRankModeChars[change.Key[1]], this.GetArgs());
                        }
                    }

                    else if (NetIRCHelper.ChannelModeChars.ContainsKey(change.Key[1]))
                    {
                        if (change.Key[0] == '+')
                        {
                            this._channelTab.AddChannelModeSet(setter.NickName, NetIRCHelper.ChannelModeChars[change.Key[1]], this.GetArgs());
                        }

                        else
                        {
                            this._channelTab.AddChannelModeRemoved(setter.NickName, NetIRCHelper.ChannelModeChars[change.Key[1]], this.GetArgs());
                        }
                    }

                    else if (change.Key[1] == 'b')
                    {
                        if (change.Key[0] == '+')
                        {
                            this._channelTab.AddBanSet(setter.NickName, change.Value, this.GetArgs());
                        }

                        else
                        {
                            this._channelTab.AddBanRemoved(setter.NickName, change.Value, this.GetArgs());
                        }
                    }
                }
            });
        }

        private void _channel_OnKick(Channel source, User kicker, User user, string reason)
        {
            this.Run(() =>
            {
                this._channelTab.AddKick(kicker.NickName, user.NickName, reason, this.GetArgs());
                this._channelTab.RemoveUser(user.ToUserItem(source));
            });
        }

        private void _channel_OnAction(Channel source, User user, string action)
        {
            this.Run(() =>
            {
                this._channelTab.AddAction(user, action, this.GetArgs());
            });
        }

        private void _channel_OnWho(Channel source, User user, string message)
        {
            if (user.IsAway)
                this._ircClient.Send(new Whois(user));
        }

        private void _channel_OnChannelOperatorNeeded(Channel source, string reason)
        {
            this.Run(() =>
            {
                this._channelTab.AddSystemErrorChat(reason, this.GetArgs());
            });
        }

        private void _channel_OnCannotSendToChannel(Channel source, string reason)
        {
            this.Run(() =>
            {
                this._channelTab.AddSystemErrorChat(reason, this.GetArgs());
            });
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            this._ircClient.ChannelLeave -= this._ircClient_ChannelLeave;

            this._channelTab.Part -= this._channelTab_Part;

            this._channel.OnAction -= this._channel_OnAction;
            this._channel.OnMessage -= this._channel_OnMessage;
            this._channel.OnJoin -= this._channel_OnJoin;
            this._channel.OnLeave -= this._channel_OnLeave;
            this._channel.OnNames -= this._channel_OnNames;
            this._channel.OnTopic -= this._channel_OnTopic;
            this._channel.OnTopicChange -= this._channel_OnTopicChange;
            this._channel.OnWho -= this._channel_OnWho;
            this._channel.OnMode -= this._channel_OnMode;
            this._channel.OnKick -= this._channel_OnKick;
        }

        #endregion
    }
}
