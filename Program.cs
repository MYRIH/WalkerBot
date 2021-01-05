using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WalkerBot.Services;

namespace WalkerBot
{
    internal class Program
    {
        private static async Task Main()
        {
            var Builder = new HostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    var Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("Config.json", false, true)
                    .Build();
                    x.AddConfiguration(Configuration);
                })
                .ConfigureLogging(x =>
                {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Debug);
                })
                .ConfigureDiscordHost<DiscordSocketClient>((Context, Config) =>
                {
                    Config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 1000,
                    };

                    Config.Token = Context.Configuration["Token"];
                })
                .UseCommandService((context, Config) =>
                {
                    Config.CaseSensitiveCommands = false;
                    Config.LogLevel = LogSeverity.Verbose;
                    Config.DefaultRunMode = RunMode.Async;
                })
                .ConfigureServices((context, Services) =>
                {
                    Services.AddHostedService<CommandHandler>();
                })
                .UseConsoleLifetime();

            var Host = Builder.Build();
            using (Host)
            {
                await Host.RunAsync();
            }
        }
    }
}