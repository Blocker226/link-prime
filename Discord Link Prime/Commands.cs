using Discord;
using Discord.Commands;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Discord_Link_Prime {
    public class SearchWarframe : ModuleBase {
        [Command("link"), Summary("Links a Warframe weapon or Warframe as an image.")]
        [Alias("warframe", "l")]
        public async Task SearchWF([Remainder, Summary("Warframe item name")] string itemName) {
            bool foundWeapon = false;
            for (int i = 0; i < Program.warframeArray.Length; i++) {
                if (itemName.Contains(Program.warframeArray[i] + "]")) {
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
                //await Context.Channel.SendMessageAsync("Sorry, that item cannot be linked to.");
            }
        }
    }

    public class CmdHelp : ModuleBase {
        [Command("help]"), Summary("Get help on how to use Link Prime")]
        public async Task Help() {
            string helpPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Help.txt");
            string[] helpDesc = File.ReadLines(helpPath).Take(3).ToArray();
            string[] cmdList = File.ReadLines(helpPath).Skip(4).ToArray();
            EmbedBuilder helpBuilder = new EmbedBuilder() {
                Title = "Link Prime A1.0.4 Help",
                Description = string.Join("\n", helpDesc),
                Color = new Color(255, 255, 0),
            };

            for (int i = 0; i < cmdList.Length; i += 2) {
                if (cmdList[i].StartsWith("[") || cmdList[i].StartsWith("wf[")) {
                    helpBuilder.AddField(x => { x.WithName(cmdList[i]); x.WithValue(cmdList[i + 1]); });
                }
            }
            await Context.Channel.SendMessageAsync("\u200B", false, helpBuilder);
        }
    }

    public class CmdInfo : ModuleBase {
        [Command("info]"), Summary("Find out about Link Prime")]
        public async Task Info() {
            string infoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Info.txt");
            EmbedBuilder infoBuilder = new EmbedBuilder() {
                Author = new EmbedAuthorBuilder() {
                    Name = "Link Prime - Warframe Chat Linker",
                    Url = "http://bitbucket.org/Blocker226/link-prime"
                },
                Description = File.ReadAllText(infoPath),
                Color = new Color(255, 255, 0),
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
}
