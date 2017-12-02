using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using TkokDiscordBot.Core.Commands.Abstractions;
using TkokDiscordBot.Core.Commands.Attributes;
using TkokDiscordBot.Core.Commands.Dto;
using TkokDiscordBot.EntGaming;
using TkokDiscordBot.EntGaming.Dto;
using TkokDiscordBot.Enums;

namespace TkokDiscordBot.Core.Commands
{
    [Priority(CommandPriority.Low)]
    internal class AutoTrackCommand : IBotCommand
    {
        private readonly EntClient _entClient;
        private readonly CurrentGameStore _currentGameStore;

        public AutoTrackCommand(EntClient entClient, CurrentGameStore currentGameStore)
        {
            _entClient = entClient;
            _currentGameStore = currentGameStore;
        }

        public async Task<bool> Handle(DiscordClient sender, MessageCreateEventArgs eventArgs)
        {
            if (eventArgs.Message.Author.IsBot || eventArgs.Message.Content.StartsWith("!"))
            {
                return false;
            }

            var message = eventArgs.Message.Content;

            var commandRegex = new Regex(@"(?:^|\s)([a-z0-9]{3,32}\d{2})(?:\s|$)").Match(message.Trim());
            if (!commandRegex.Success)
            {
                return false;
            }

            var gameName = commandRegex.Result("$1").Trim();

            if (!string.IsNullOrWhiteSpace(gameName))
            {
                _currentGameStore .Status = new LobbyStatus(gameName);

                await eventArgs.Message.RespondAsync($"<:bot:379752914215763994> Detected game name. :eye: Monitoring game `{gameName}`.");
            }

            return true;
        }

        public CommandInfo GetUsage()
        {
            return null;
        }
    }
}
