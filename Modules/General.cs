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
        private readonly ILogger<General> Logger;

        public General(ILogger<General> NewLogger)
            => Logger = NewLogger;

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
            var embed = new EmbedBuilder();
            SocketGuild guild = Context.Guild;
            var users = guild.Users;
            List<SocketGuildUser> Students = new List<SocketGuildUser> { };
            List<string> ValidStudents = new List<string> { };
            string FilePath = @"C:\Dev\WalkerBot\WalkerBot\Students.txt";

            if (ClassArg == 40)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-40");

                foreach (SocketGuildUser user in users)
                {
                    if (user.Roles.Contains(role) && !user.IsBot)
                    {
                        Students.Add(user);
                    }
                }
                foreach (SocketGuildUser Student in Students)
                {
                    if (Student.Nickname != null)
                    {
                        ValidStudents.Add(Student.Nickname);
                    }
                    else ValidStudents.Add(Student.Username);
                }
                ValidStudents.Sort();
                using (var sw = new StreamWriter(FilePath, false))
                {
                    foreach (string temp in ValidStudents)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }

                await Context.Channel.SendFileAsync(FilePath);
            }

            if (ClassArg == 41)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-41");

                foreach (SocketGuildUser user in users)
                {
                    if (user.Roles.Contains(role) && !user.IsBot)
                    {
                        Students.Add(user);
                    }
                }
                foreach (SocketGuildUser Student in Students)
                {
                    if (Student.Nickname != null)
                    {
                        ValidStudents.Add(Student.Nickname);
                    }
                    else ValidStudents.Add(Student.Username);
                }
                ValidStudents.Sort();
                using (var sw = new StreamWriter(FilePath, false))
                {
                    foreach (string temp in ValidStudents)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }

                await Context.Channel.SendFileAsync(FilePath);
            }

            if (ClassArg == 26)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-26");

                foreach (SocketGuildUser user in users)
                {
                    if (user.Roles.Contains(role) && !user.IsBot)
                    {
                        Students.Add(user);
                    }
                }
                foreach (SocketGuildUser Student in Students)
                {
                    if (Student.Nickname != null)
                    {
                        ValidStudents.Add(Student.Nickname);
                    }
                    else ValidStudents.Add(Student.Username);
                }
                ValidStudents.Sort();
                using (var sw = new StreamWriter(FilePath, false))
                {
                    foreach (string temp in ValidStudents)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }

                await Context.Channel.SendFileAsync(FilePath);
            }

            if (ClassArg == 45)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-45");

                foreach (SocketGuildUser user in users)
                {
                    if (user.Roles.Contains(role) && !user.IsBot)
                    {
                        Students.Add(user);
                    }
                }
                foreach (SocketGuildUser Student in Students)
                {
                    if (Student.Nickname != null)
                    {
                        ValidStudents.Add(Student.Nickname);
                    }
                    else ValidStudents.Add(Student.Username);
                }
                ValidStudents.Sort();
                using (var sw = new StreamWriter(FilePath, false))
                {
                    foreach (string temp in ValidStudents)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }

                await Context.Channel.SendFileAsync(FilePath);
            }

            if (ClassArg == 1)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-1");

                foreach (SocketGuildUser user in users)
                {
                    if (user.Roles.Contains(role) && !user.IsBot)
                    {
                        Students.Add(user);
                    }
                }
                foreach (SocketGuildUser Student in Students)
                {
                    if (Student.Nickname != null)
                    {
                        ValidStudents.Add(Student.Nickname);
                    }
                    else ValidStudents.Add(Student.Username);
                }
                ValidStudents.Sort();
                using (var sw = new StreamWriter(FilePath, false))
                {
                    foreach (string temp in ValidStudents)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }

                await Context.Channel.SendFileAsync(FilePath);
            }

            if (ClassArg == 50)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Name == "IS-50");

                foreach (SocketGuildUser user in users)
                {
                    if (user.Roles.Contains(role) && !user.IsBot)
                    {
                        Students.Add(user);
                    }
                }
                foreach (SocketGuildUser Student in Students)
                {
                    if (Student.Nickname != null)
                    {
                        ValidStudents.Add(Student.Nickname);
                    }
                    else ValidStudents.Add(Student.Username);
                }
                ValidStudents.Sort();
                using (var sw = new StreamWriter(FilePath, false))
                {
                    foreach (string temp in ValidStudents)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }

                await Context.Channel.SendFileAsync(FilePath);
            }
        }
    }
}