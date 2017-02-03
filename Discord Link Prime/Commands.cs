using Discord.Commands;
using System.IO;
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
            await Context.Channel.SendMessageAsync("```Markdown\n" + File.ReadAllText(helpPath) + "\n```");
        }
    }

    public class CmdInfo : ModuleBase {
        [Command("info]"), Summary("Find out about Link Prime")]
        public async Task Help() {
            string helpPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Info.txt");
            await Context.Channel.SendMessageAsync("```Markdown\n" + File.ReadAllText(helpPath) + "\n```");
        }
    }

    public class CmdDonate : ModuleBase {
        [Command("donate]"), Summary("Donate to the author!")]
        public async Task Help() {
            await Context.Channel.SendMessageAsync("If you like Link Prime, do consider donating: <https://www.paypal.me/Blocker226>");
        }
    }
}
