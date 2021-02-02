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
    public class Student
    {
        private string Name;
        private int Cumulative;

        public Student(string NewName, int NewCumulative)
        {
            Name = NewName;
            Cumulative = NewCumulative;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetCumulative()
        {
            return Cumulative;
        }

        public int SetCumulative(int NewCumulative)
        {
            return Cumulative = NewCumulative;
        }
    }

    public class General : ModuleBase<SocketCommandContext>
    {
        public static List<Student> Students45 = new List<Student> { };
        public static List<Student> Students41 = new List<Student> { };
        public static List<Student> Students50 = new List<Student> { };
        public static List<Student> Students1 = new List<Student> { };

        public static List<string> WorkShopStudentsNames = new List<string> { };

        public static List<string> Valid41Students = new List<string> { };
        public static List<string> Valid45Students = new List<string> { };
        public static List<string> Valid1Students = new List<string> { };
        public static List<string> Valid50Students = new List<string> { };

        public static List<string> Messages45 = new List<string> { };
        public static List<string> Messages41 = new List<string> { };
        public static List<string> Messages50 = new List<string> { };
        public static List<string> Messages1 = new List<string> { };

        private readonly ILogger<General> Logger;

        public string FilePath41 = @"C:\Dev\WalkerBot\WalkerBot\41Students.txt";
        public string FilePath45 = @"C:\Dev\WalkerBot\WalkerBot\45Students.txt";
        public string FilePath1 = @"C:\Dev\WalkerBot\WalkerBot\1Students.txt";
        public string FilePath50 = @"C:\Dev\WalkerBot\WalkerBot\50Students.txt";

        public string FilePathWKShp = @"C:\Dev\WalkerBot\WalkerBot\WorkShopAttendance.txt";

        public string FilePath41Cumul = @"C:\Dev\WalkerBot\WalkerBot\41StudentsCumul.txt";
        public string FilePath45Cumul = @"C:\Dev\WalkerBot\WalkerBot\45StudentsCumul.txt";
        public string FilePath1Cumul = @"C:\Dev\WalkerBot\WalkerBot\1StudentsCumul.txt";
        public string FilePath50Cumul = @"C:\Dev\WalkerBot\WalkerBot\50StudentsCumul.txt";

        public General(ILogger<General> NewLogger)
            => Logger = NewLogger;

        public static bool first = false;

        [Command("wkshp")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task WorkShopAttendance()
        {
            if (first)
            {
                using (var sw = new StreamWriter(FilePathWKShp, false))
                {
                    foreach (string temp in WorkShopStudentsNames)
                    {
                        await sw.WriteLineAsync(temp);
                    }
                }
                WorkShopStudentsNames.Clear();
                first = false;
                await Context.Channel.SendFileAsync(FilePathWKShp);
            }
            else
            {
                first = true;
            }
        }

        [Command("here")]
        public async Task Here(int arg)
        {
            if (Context.Channel.Id == 795497906978684929)
            {
                if (first)
                {
                    var GuildUser = Context.User as SocketGuildUser;
                    string name;
                    if (GuildUser.Nickname != null)
                    {
                        if (WorkShopStudentsNames.Contains(GuildUser.Nickname))
                        {
                            await Context.Channel.SendMessageAsync("You've already been accounted for.");
                        }
                        else
                        {
                            name = $"{GuildUser.Nickname}  {arg}";
                            WorkShopStudentsNames.Add(name);
                            await Context.Channel.SendMessageAsync("You've been added.");
                        }
                    }
                    else if (GuildUser.Username != null)
                    {
                        if (WorkShopStudentsNames.Contains(GuildUser.Username))
                        {
                            await Context.Channel.SendMessageAsync("You've already been accounted for.");
                        }
                        else
                        {
                            name = $"{GuildUser.Username}  {arg}";
                            WorkShopStudentsNames.Add(name);
                            await Context.Channel.SendMessageAsync("You've been added.");
                        }
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

        [Command("cumul")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Cumulative(int ClassArg)
        {
            if (ClassArg == 41)
            {
                List<Student> Sorted = Students41.OrderBy(temp => temp.GetName()).ToList();
                using (var sw = new StreamWriter(FilePath41Cumul, false))
                {
                    foreach (Student temp in Sorted)
                    {
                        await sw.WriteLineAsync(temp.GetName() + temp.GetCumulative());
                    }
                }
                await Context.Channel.SendFileAsync(FilePath41Cumul);
            }
            if (ClassArg == 45)
            {
                List<Student> Sorted = Students45.OrderBy(temp => temp.GetName()).ToList();
                using (var sw = new StreamWriter(FilePath45Cumul, false))
                {
                    foreach (Student temp in Sorted)
                    {
                        await sw.WriteLineAsync(temp.GetName() + temp.GetCumulative());
                    }
                }
                await Context.Channel.SendFileAsync(FilePath45Cumul);
            }
            if (ClassArg == 1)
            {
                List<Student> Sorted = Students1.OrderBy(temp => temp.GetName()).ToList();
                using (var sw = new StreamWriter(FilePath1Cumul, false))
                {
                    foreach (Student temp in Sorted)
                    {
                        await sw.WriteLineAsync(temp.GetName() + temp.GetCumulative());
                    }
                }
                await Context.Channel.SendFileAsync(FilePath1Cumul);
            }
            if (ClassArg == 50)
            {
                List<Student> Sorted = Students50.OrderBy(temp => temp.GetName()).ToList();
                using (var sw = new StreamWriter(FilePath50Cumul, false))
                {
                    foreach (Student temp in Sorted)
                    {
                        await sw.WriteLineAsync(temp.GetName() + temp.GetCumulative());
                    }
                }
                await Context.Channel.SendFileAsync(FilePath50Cumul);
            }
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
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-41");
                ulong LectureChannel = 795497257671196683;
                ulong HelpChannel = 795497568112607242;
                int limit = 1000;

                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                var templist = ChannelMessages.ToList();
                templist.AddRange(await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync());

                List<IMessage> Valid41Messages = new List<IMessage> { };
                foreach (var msg in templist)
                {
                    if (Messages41.Contains(msg.Content))
                    {
                        continue;
                    }
                    else
                    {
                        Valid41Messages.Add(msg);
                    }
                }

                if (Valid41Messages != null)
                {
                    foreach (var msg in Valid41Messages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (user != null)
                        {
                            if (Valid41Students.Contains(user.Username) || Valid41Students.Contains(user.Nickname))
                                continue;
                            else
                            {
                                if (user.Roles.Contains(role) && !user.IsBot)
                                {
                                    if (user.Nickname != null)
                                    {
                                        Student temp = new Student(user.Nickname, 1);
                                        var match = Students41.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students41.Add(temp);
                                        }

                                        Valid41Students.Add(user.Nickname);
                                    }
                                    else
                                    {
                                        Student temp = new Student(user.Username, 1);
                                        var match = Students41.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students41.Add(temp);
                                        }
                                        Valid41Students.Add(user.Username);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var msg in templist)
                {
                    Messages41.Add(msg.ToString());
                }

                Valid41Students.Sort();

                using (var sw = new StreamWriter(FilePath41, false))
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
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-45");
                ulong LectureChannel = 795497409873838121;
                ulong HelpChannel = 795497584562929664;
                int limit = 1000;

                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                var templist = ChannelMessages.ToList();
                templist.AddRange(await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync());

                List<IMessage> Valid45Messages = new List<IMessage> { };
                foreach (var msg in templist)
                {
                    if (Messages45.Contains(msg.Content))
                    {
                        continue;
                    }
                    else
                    {
                        Valid45Messages.Add(msg);
                    }
                }

                if (Valid45Messages != null)
                {
                    foreach (var msg in Valid45Messages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (user != null)
                        {
                            if (Valid45Students.Contains(user.Username) || Valid45Students.Contains(user.Nickname))
                                continue;
                            else
                            {
                                if (user.Roles.Contains(role) && !user.IsBot)
                                {
                                    if (user.Nickname != null)
                                    {
                                        Student temp = new Student(user.Nickname, 1);
                                        var match = Students45.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students45.Add(temp);
                                        }
                                        Valid45Students.Add(user.Nickname);
                                    }
                                    else
                                    {
                                        Student temp = new Student(user.Username, 1);
                                        var match = Students45.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students45.Add(temp);
                                        }
                                        Valid45Students.Add(user.Username);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var msg in templist)
                {
                    Messages45.Add(msg.ToString());
                }

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
                var role = guild.Roles.FirstOrDefault(x => x.Name == "CSCI-1");
                ulong LectureChannel = 795497373001711626;
                ulong HelpChannel = 795497526299590696;
                int limit = 950;

                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                var templist = ChannelMessages.ToList();
                templist.AddRange(await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync());

                List<IMessage> Valid1Messages = new List<IMessage> { };
                foreach (var msg in templist)
                {
                    if (Messages1.Contains(msg.Content))
                    {
                        continue;
                    }
                    else
                    {
                        Valid1Messages.Add(msg);
                    }
                }

                if (Valid1Messages != null)
                {
                    foreach (var msg in Valid1Messages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (user != null)
                        {
                            if (Valid1Students.Contains(user.Username) || Valid1Students.Contains(user.Nickname))
                                continue;
                            else
                            {
                                if (user.Roles.Contains(role) && !user.IsBot)
                                {
                                    if (user.Nickname != null)
                                    {
                                        Student temp = new Student(user.Nickname, 1);
                                        var match = Students1.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students1.Add(temp);
                                        }
                                        Valid1Students.Add(user.Nickname);
                                    }
                                    else
                                    {
                                        Student temp = new Student(user.Username, 1);
                                        var match = Students1.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students1.Add(temp);
                                        }
                                        Valid1Students.Add(user.Username);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var msg in templist)
                {
                    Messages1.Add(msg.ToString());
                }

                Valid1Students.Sort();

                using (var sw = new StreamWriter(FilePath1, false))
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
                var role = guild.Roles.FirstOrDefault(x => x.Name == "IS-50");
                ulong LectureChannel = 795497426596921384;
                ulong HelpChannel = 795497599598723114;
                int limit = 950;
                var ChannelMessages = await guild.GetTextChannel(LectureChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync();
                var templist = ChannelMessages.ToList();
                templist.AddRange(await guild.GetTextChannel(HelpChannel).GetMessagesAsync(LastMessage.Id, Direction.Before, limit).FlattenAsync());

                List<IMessage> Valid50Messages = new List<IMessage> { };
                foreach (var msg in templist)
                {
                    if (Messages50.Contains(msg.Content))
                    {
                        continue;
                    }
                    else
                    {
                        Valid50Messages.Add(msg);
                    }
                }

                if (Valid50Messages != null)
                {
                    foreach (var msg in Valid50Messages)
                    {
                        var user = (msg.Author as SocketGuildUser);
                        if (user != null)
                        {
                            if (Valid50Students.Contains(user.Username) || Valid50Students.Contains(user.Nickname))
                                continue;
                            else
                            {
                                if (user.Roles.Contains(role) && !user.IsBot)
                                {
                                    if (user.Nickname != null)
                                    {
                                        Student temp = new Student(user.Nickname, 1);
                                        var match = Students50.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students50.Add(temp);
                                        }
                                        Valid50Students.Add(user.Nickname);
                                    }
                                    else
                                    {
                                        Student temp = new Student(user.Username, 1);
                                        var match = Students50.Find(check => check.GetName() == temp.GetName());
                                        if (match != null)
                                        {
                                            int newcum = match.GetCumulative();
                                            newcum++;
                                            match.SetCumulative(newcum);
                                        }
                                        else
                                        {
                                            Students50.Add(temp);
                                        }
                                        Valid50Students.Add(user.Username);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var msg in templist)
                {
                    Messages50.Add(msg.ToString());
                }

                Valid50Students.Sort();

                using (var sw = new StreamWriter(FilePath50, false))
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