using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using DSharpPlus.Entities;
using TkokDiscordBot.Configuration;
using TkokDiscordBot.Extensions;

namespace TkokDiscordBot.EntGaming
{
    internal class GameStatusUpdater
    {
        private readonly EntClient _entClient;
        private readonly ISettings _settings;
        private readonly CurrentGameStore _currentGameStore;
        private IDictionary<DiscordChannel, string> _channelWithDefaultTopic;

        public GameStatusUpdater(EntClient entClient, ISettings settings, CurrentGameStore currentGameStore)
        {
            _entClient = entClient;
            _settings = settings;
            _currentGameStore = currentGameStore;
        }

        private async void OnLobbyStatusChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName != nameof(_currentGameStore.Status)) return;

            var lobbyInfo = _currentGameStore.Status;

            if (lobbyInfo != null)
            {
                //await _mainChannel.ModifyAsync("main", null, "Currently hosting: " + lobbyInfo);

                foreach (var channelData in _channelWithDefaultTopic)
                {
                    await channelData.Key.ModifyAsync(null, null, "Currently hosting: " + lobbyInfo);
                }
            }
            else
            {
                //await _mainChannel.ModifyAsync("main", null, _defaultMainChannelTopic);

                foreach (var channelData in _channelWithDefaultTopic)
                {
                    await channelData.Key.ModifyAsync(null, null, channelData.Value);
                }
            }
        }

        public async void StartUpdating(IEnumerable<DiscordChannel> subscribedChannels)
        {
            if (subscribedChannels == null)
                throw new ArgumentNullException(nameof(subscribedChannels));

            _channelWithDefaultTopic = new Dictionary<DiscordChannel, string>();
            foreach (var channel in subscribedChannels)
            {
                _channelWithDefaultTopic.Add(channel, channel.Topic);
            }
            _currentGameStore.LobbyStatusChanged += OnLobbyStatusChanged;

            while (true)
            {
                var lobbyStatus = _currentGameStore.Status;
                if (lobbyStatus != null)
                {
                    var findGameBy = lobbyStatus.Id != null ? lobbyStatus.Id.ToString() : lobbyStatus.GameName;
                    var status = await _entClient.GetBotStatus(findGameBy);

                    //Maybe the game is being hosted, let's wait a bit before purging the status
                    if (status == null &&
                        _currentGameStore.Status?.GameName != null &&
                        _currentGameStore.Status.GameName.StartsWith(_settings.EntUsername) &&
                        _currentGameStore.TrackingDate.ElapsedFromNow() < TimeSpan.FromMinutes(1))
                    {
                        continue;

                    }
                    _currentGameStore.Status = status;
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}
