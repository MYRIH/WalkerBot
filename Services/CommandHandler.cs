using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace WalkerBot.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DiscordSocketClient Client;
        private readonly CommandService Service;
        private readonly IConfiguration Config;

        public CommandHandler(IServiceProvider NewProvider, DiscordSocketClient NewClient, CommandService NewService, IConfiguration NewConfig)
        {
            ServiceProvider = NewProvider;
            Client = NewClient;
            Service = NewService;
            Config = NewConfig;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            Client.MessageReceived += OnMessageReceived;
            Service.CommandExecuted += OnCommandExecuted;
            await Service.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider);
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(Config["prefix"], ref argPos) && !message.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(Client, message);
            await Service.ExecuteAsync(context, argPos, ServiceProvider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess) await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}