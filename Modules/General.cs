using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace WalkerBot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<General> Logger;

        public General(ILogger<General> NewLogger)
            => Logger = NewLogger;

        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong");
            Logger.LogInformation($"{Context.User.Username} executed the ping command");
        }

        [Command("info")]
        public async Task Info(SocketGuildUser GuildUser = null)
        {
            //?? is kinda like a ternary operator
            if (GuildUser == null)
            {
                var Builder = new EmbedBuilder().WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                    .WithDescription("Description")
                    .WithColor(new Color(33, 176, 252))
                    .AddField("User ID: ", Context.User.Id, true)
                    .AddField("Discriminator: ", Context.User.Discriminator, true)
                    .AddField("Created at: ", Context.User.CreatedAt.ToString("MM/dd/yyyy"), true)
                    .AddField("Joined at: ", (Context.User as SocketGuildUser).JoinedAt.Value.ToString("MM/dd/yyyy"), true)
                    .AddField("Roles: ", string.Join(" ", (Context.User as SocketGuildUser).Roles.Select(x => x.Mention)))
                    .WithCurrentTimestamp();
                var Embed = Builder.Build();
                await Context.Channel.SendMessageAsync(null, false, Embed);
            }
            else
            {
                var Builder = new EmbedBuilder().WithThumbnailUrl(GuildUser.GetAvatarUrl() ?? GuildUser.GetDefaultAvatarUrl())
                       .WithDescription("Description")
                       .WithColor(new Color(33, 176, 252))
                       .AddField("User ID: ", GuildUser.Id, true)
                       .AddField("Discriminator: ", GuildUser.Discriminator, true)
                       .AddField("Created at: ", GuildUser.CreatedAt.ToString("MM/dd/yyyy"), true)
                       .AddField("Joined at: ", GuildUser.JoinedAt.Value.ToString("MM/dd/yyyy"), true)
                       .AddField("Roles: ", string.Join(" ", GuildUser.Roles.Select(x => x.Mention)))
                       .WithCurrentTimestamp();
                var Embed = Builder.Build();
                await Context.Channel.SendMessageAsync(null, false, Embed);
            }
            Logger.LogInformation($"{Context.User.Username} executed the info command");
        }

        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int AmountToPurge)
        {
            var MessagesToPurge = await Context.Channel.GetMessagesAsync(AmountToPurge + 1).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(MessagesToPurge);

            var ConfirmationMessage = await Context.Channel.SendMessageAsync($"{MessagesToPurge.Count()} messages deleted successfully");
            await Task.Delay(2500);
            await ConfirmationMessage.DeleteAsync();
            Logger.LogInformation($"{Context.User.Username} executed the purge command");
        }

        [Command("atd")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Attendance(int ClassArg)
        {
            List<SocketGuildUser> ListOfStudents;
        }
    }
}