using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace WalkerBot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        public static List<string> Valid41Students = new List<string> { };
        public static List<string> Valid45Students = new List<string> { };
        public static List<string> Valid26Students = new List<string> { };
        public static List<string> Valid1Students = new List<string> { };
        public static List<string> Valid50Students = new List<string> { };

        private readonly ILogger<General> Logger;

        public string FilePath41 = @"C:\Dev\WalkerBot\WalkerBot\41Students.txt";
        public string FilePath45 = @"C:\Dev\WalkerBot\WalkerBot\45Students.txt";
        public string FilePath1 = @"C:\Dev\WalkerBot\WalkerBot\1Students.txt";
        public string FilePath50 = @"C:\Dev\WalkerBot\WalkerBot\50Students.txt";

        public General(ILogger<General> NewLogger)
            => Logger = NewLogger;

        [Command("here")]
        public async Task Here()
        {
            var GuildUser = Context.User as SocketGuildUser;
            if (GuildUser.Roles.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == "CSCI-41")))
            {
                if (GuildUser.Nickname != null)
                {
                    if (Valid41Students.Contains(GuildUser.Nickname))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid41Students.Contains(GuildUser.Nickname))
                    {
                        Valid41Students.Add(GuildUser.Nickname);
                    }
                }
                else
                {
                    if (Valid41Students.Contains(GuildUser.Username))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid41Students.Contains(GuildUser.Username))
                    {
                        Valid41Students.Add(GuildUser.Username);
                    }
                }
            }
            else if (GuildUser.Roles.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == "CSCI-45")))
            {
                if (GuildUser.Nickname != null)
                {
                    if (Valid45Students.Contains(GuildUser.Nickname))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid45Students.Contains(GuildUser.Nickname))
                    {
                        Valid45Students.Add(GuildUser.Nickname);
                    }
                }
                else
                {
                    if (Valid45Students.Contains(GuildUser.Username))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid45Students.Contains(GuildUser.Username))
                    {
                        Valid45Students.Add(GuildUser.Username);
                    }
                }
            }
            else if (GuildUser.Roles.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == "CSCI-1")))
            {
                if (GuildUser.Nickname != null)
                {
                    if (Valid1Students.Contains(GuildUser.Nickname))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid1Students.Contains(GuildUser.Nickname))
                    {
                        Valid1Students.Add(GuildUser.Nickname);
                    }
                }
                else
                {
                    if (Valid1Students.Contains(GuildUser.Username))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid1Students.Contains(GuildUser.Username))
                    {
                        Valid1Students.Add(GuildUser.Username);
                    }
                }
            }
            else if (GuildUser.Roles.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == "IS-50")))
            {
                if (GuildUser.Nickname != null)
                {
                    if (Valid50Students.Contains(GuildUser.Nickname))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid50Students.Contains(GuildUser.Nickname))
                    {
                        Valid50Students.Add(GuildUser.Nickname);
                    }
                }
                else
                {
                    if (Valid50Students.Contains(GuildUser.Username))
                    {
                        await Context.Channel.SendMessageAsync("You're already been accounted for.");
                    }
                    else if (!Valid50Students.Contains(GuildUser.Username))
                    {
                        Valid50Students.Add(GuildUser.Username);
                    }
                }
            }
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

        [Command("atd")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Attendance(int ClassArg)
        {
            var LastMessage = await Context.Channel.SendMessageAsync("Getting Attendance");
            SocketGuild guild = Context.Guild;
            var users = guild.Users;

            if (ClassArg == 41)
            {
                //await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-41");
                ulong LectureChannel = 795497257671196683;
                //ulong HelpChannel = 795497568112607242;
                int limit = 150;
                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                //ChannelMessages = await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                if (ChannelMessages != null)
                {
                    foreach (var msg in ChannelMessages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (Valid41Students.Contains(user.Username) || Valid41Students.Contains(user.Nickname))
                            continue;
                        else
                        {
                            if (user.Roles.Contains(role) && !user.IsBot)
                            {
                                if (user.Nickname != null)
                                {
                                    Valid41Students.Add(user.Nickname);
                                }
                                else { Valid41Students.Add(user.Username); }
                            }
                        }
                    }
                }

                // await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                Valid41Students.Sort();
                using (var sw = new StreamWriter(FilePath41, true))
                {
                    foreach (string temp in Valid41Students)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }
                Valid41Students.Clear();
                await Context.Channel.SendFileAsync(FilePath41);
            }
            else if (ClassArg == 45)
            {
                //await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-45");
                ulong LectureChannel = 795497409873838121;
                //ulong HelpChannel = 795497568112607242;
                int limit = 150;
                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                //ChannelMessages = await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                if (ChannelMessages != null)
                {
                    foreach (var msg in ChannelMessages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (Valid45Students.Contains(user.Username) || Valid45Students.Contains(user.Nickname))
                            continue;
                        else
                        {
                            if (user.Roles.Contains(role) && !user.IsBot)
                            {
                                if (user.Nickname != null)
                                {
                                    Valid45Students.Add(user.Nickname);
                                }
                                else { Valid45Students.Add(user.Username); }
                            }
                        }
                    }
                }

                // await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                Valid45Students.Sort();
                using (var sw = new StreamWriter(FilePath45, false))
                {
                    foreach (string temp in Valid45Students)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }
                Valid45Students.Clear();
                await Context.Channel.SendFileAsync(FilePath45);
            }
            else if (ClassArg == 1)
            {
                //await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-1");
                ulong LectureChannel = 795497373001711626;
                //ulong HelpChannel = 795497568112607242;
                int limit = 50;
                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                //ChannelMessages = await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                if (ChannelMessages != null)
                {
                    foreach (var msg in ChannelMessages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (Valid1Students.Contains(user.Username) || Valid1Students.Contains(user.Nickname))
                            continue;
                        else
                        {
                            if (user.Roles.Contains(role) && !user.IsBot)
                            {
                                if (user.Nickname != null)
                                {
                                    Valid1Students.Add(user.Nickname);
                                }
                                else { Valid1Students.Add(user.Username); }
                            }
                        }
                    }
                }

                // await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                Valid1Students.Sort();
                using (var sw = new StreamWriter(FilePath45, true))
                {
                    foreach (string temp in Valid1Students)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }
                Valid1Students.Clear();
                await Context.Channel.SendFileAsync(FilePath1);
            }
            else if (ClassArg == 50)
            {
                //await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                var role = guild.Roles.FirstOrDefault(x => x.Name == "IS-50");
                ulong LectureChannel = 795497426596921384;
                //ulong HelpChannel = 795497568112607242;
                int limit = 45;
                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                //ChannelMessages = await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                if (ChannelMessages != null)
                {
                    foreach (var msg in ChannelMessages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (Valid50Students.Contains(user.Username) || Valid50Students.Contains(user.Nickname))
                            continue;
                        else
                        {
                            if (user.Roles.Contains(role) && !user.IsBot)
                            {
                                if (user.Nickname != null)
                                {
                                    Valid50Students.Add(user.Nickname);
                                }
                                else { Valid50Students.Add(user.Username); }
                            }
                        }
                    }
                }

                // await Context.Channel.SendMessageAsync($"{Valid41Students.Count}");
                Valid50Students.Sort();
                using (var sw = new StreamWriter(FilePath50, true))
                {
                    foreach (string temp in Valid50Students)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }
                Valid50Students.Clear();
                await Context.Channel.SendFileAsync(FilePath50);
            }
        }
    }
}