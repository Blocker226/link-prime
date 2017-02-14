using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Discord_Link_Prime {
    public class SearchWarframe : ModuleBase {
        [Command("link"), Summary("Links a Warframe weapon or Warframe as an image."), Alias("warframe", "l")]
        public async Task SearchWF([Remainder, Summary("Warframe item name")] string itemName) {
            bool foundWeapon = false;
            for (int i = 0; i < Program.warframeArray.Length; i++) {
                if (itemName == Program.warframeArray[i] + "]") {
                    string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Images\" + Program.warframeArray[i] + ".png");
                    //Console.WriteLine(path);
                    if (File.Exists(path)) {
                        await Context.Channel.SendFileAsync(path);
                    }
                    else {
                        await Context.Channel.SendMessageAsync("Sorry, there's no image available for this item.");
                    }
                    foundWeapon = true;
                    break;
                }

            }
            if (!foundWeapon) {
                if (itemName == "John Prodman]") {
                    await Context.Channel.SendMessageAsync("The man, the myth, the legend.");
                    foundWeapon = true;
                }
                else if (itemName == "Clem]") {
                    await Context.Channel.SendMessageAsync("He's a special guy, you know. Little weird but a hell of a fighter.");
                    foundWeapon = true;
                }
                else {
                    await Context.Channel.SendMessageAsync("Sorry, that item cannot be linked to.");
                }
            }
        }
    }

    public class CmdHelp : ModuleBase {
        [Command("help]"), Summary("Get help on how to use Link Prime!")]
        public async Task Help() {
            string helpPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Help.txt");
            string[] helpDesc = File.ReadLines(helpPath).Take(3).ToArray();
            string[] cmdList = File.ReadLines(helpPath).Skip(4).ToArray();
            EmbedBuilder helpBuilder = new EmbedBuilder() {
                Author = new EmbedAuthorBuilder() {
                    Name = "Link Prime A1.0.7 Help",
                    IconUrl = "http://i.imgur.com/MG0pc7Q.png",
                },
                Description = string.Join("\n", helpDesc),
                Color = new Color(250, 246, 122),
            };

            for (int i = 0; i < Program.commandNames.Length; i++) {
                if (i == 0) {
                    helpBuilder.AddField(x => { x.WithName(cmdList[0]); x.WithValue(Program.commandSummaries[i]); });
                    helpBuilder.AddField(x => { x.WithName(cmdList[1]); x.WithValue(Program.commandSummaries[i] + " This command can be used mid-sentence."); });
                }
                else {
                    helpBuilder.AddField(x => { x.WithName("[" + Program.commandNames[i]); x.WithValue(Program.commandSummaries[i]); });
                }
            }
            await Context.Channel.SendMessageAsync("\u200B", false, helpBuilder);
        }
    }

    public class CmdInfo : ModuleBase {
        [Command("info]"), Summary("Find out about Link Prime!")]
        public async Task Info() {
            string infoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Info.txt");
            EmbedBuilder infoBuilder = new EmbedBuilder() {
                Author = new EmbedAuthorBuilder() {
                    Name = "Link Prime - Warframe Chat Linker",
                    Url = "http://bitbucket.org/Blocker226/link-prime"
                },
                ThumbnailUrl = "http://i.imgur.com/MG0pc7Q.png",
                Description = File.ReadAllText(infoPath),
                Color = new Color(250, 246, 122),
            };
            await Context.Channel.SendMessageAsync("\u200B", false, infoBuilder);
        }
    }

    public class CmdDonate : ModuleBase {
        [Command("donate]"), Summary("Donate to the author!")]
        public async Task Donate() {
            await Context.Channel.SendMessageAsync("\u200BIf you like Link Prime, do consider donating: <https://www.paypal.me/Blocker226>");
        }
    }

    public class CmdInvite : ModuleBase {
        [Command("invite]"), Summary("Invite Link Prime to your server!")]
        public async Task Invite() {
            IDMChannel dmChannel = await Context.User.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync("\u200BThe Void Relic has been cracked open. Add Link Prime via\nhttps://discordapp.com/oauth2/authorize?client_id=276370292052459523&scope=bot&permissions=35840");
        }
    }

    public class CmdStats : ModuleBase {
        [Command("stats]"), Summary("Check out Link Prime's stats!")]
        public async Task Stats() {
            TimeSpan convertedUpTime = TimeSpan.FromMinutes(Program.currentUptime);
            TimeSpan convertedTotalUpTime = TimeSpan.FromMinutes(Program.loadedStats.totalUptime);
            string convertedUpTimeString = string.Format("{0:d'days 'h'hrs 'mm'mins'}", convertedUpTime);
            string convertedTotalUpTimeString = string.Format("{0:d'days 'h'hrs 'mm'mins'}", convertedTotalUpTime);
            EmbedBuilder statsBuilder = new EmbedBuilder() {
                Author = new EmbedAuthorBuilder() {
                    Name = "Link Prime A1.07 Stats",
                    IconUrl = "http://i.imgur.com/MG0pc7Q.png",
                },
                Description = "Here's some interesting numbers! Last stat reset: 9/Feb/2017",
                Color = new Color(250, 246, 122)
            }.AddField(x => { x.WithName("Servers"); x.WithValue(Program.loadedStats.serversJoined.ToString()); x.WithIsInline(true); })
            .AddField(x => { x.WithName("Uptime"); x.WithValue(convertedUpTimeString); x.WithIsInline(true); })
            .AddField(x => { x.WithName("Total Uptime"); x.WithValue(convertedTotalUpTimeString); x.WithIsInline(true); });

            await Context.Channel.SendMessageAsync("\u200B", false, statsBuilder);
        }
    }
}
