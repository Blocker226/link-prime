using Discord;
using Discord.Commands;
using Discord.Addons.InteractiveCommands;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord.Rest;

namespace Discord_Link_Prime {
    public class SearchWarframe : ModuleBase {
        //Message ID for react reaction
        ulong displayID;
        string lastImage;

        [Command("link"), Summary("Links a Warframe weapon or Warframe as an image."), Alias("warframe", "l")]
        public async Task SearchWF([Remainder, Summary("Warframe item name")] string itemName) {

            System.Globalization.TextInfo nameConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            itemName = nameConverter.ToTitleCase(itemName);
            if (Program.weaponArray != null) {
                //First search weapons
                for (int i = 0; i < Program.weaponArray.Length; i++) {
                    if (itemName == Program.weaponArray[i].name + "]") {
                        EmbedBuilder itemBuilder = new EmbedBuilder() {
                            Author = new EmbedAuthorBuilder() {
                                Name = Program.weaponArray[i].name
                            },
                            ThumbnailUrl = Program.weaponArray[i].iconURL,
                            Description = Program.weaponArray[i].description,
                            Color = new Color(250, 246, 122),
                        }.AddField(x => { x.WithName("Type"); x.WithValue(Program.weaponArray[i].type); });
                        IMessage sentMsg = await Context.Channel.SendMessageAsync("", false, itemBuilder);

                        //Setup reaction listener
                        displayID = sentMsg.Id;
                        lastImage = Program.weaponArray[i].iconURL;
                        var theMsg = (RestUserMessage)await sentMsg.Channel.GetMessageAsync(sentMsg.Id);
                        Program.bot.ReactionAdded += Bot_ReactionAdded;
                        await theMsg.AddReactionAsync("🔍");
                        return;
                    }
                }
            }
            if (Program.warframeArray != null) {
                //Second search warframes
                for (int i = 0; i < Program.warframeArray.Length; i++) {
                    if (itemName == Program.warframeArray[i].name + "]") {
                        EmbedBuilder itemBuilder = new EmbedBuilder() {
                            Author = new EmbedAuthorBuilder() {
                                Name = Program.warframeArray[i].name
                            },
                            ThumbnailUrl = Program.warframeArray[i].iconURL,
                            Description = Program.warframeArray[i].description,
                            Color = new Color(250, 246, 122),
                        }.AddField(x => { x.WithName("Armour"); x.WithValue(Program.warframeArray[i].maxArmour.ToString()); })
                        .AddField(x => { x.WithName("Health"); x.WithValue(Program.warframeArray[i].maxHealth.ToString()); });
                        IMessage sentMsg = await Context.Channel.SendMessageAsync("", false, itemBuilder);

                        //Setup reaction listener
                        displayID = sentMsg.Id;
                        lastImage = Program.warframeArray[i].iconURL;
                        var theMsg = (RestUserMessage)await sentMsg.Channel.GetMessageAsync(sentMsg.Id);
                        Program.bot.ReactionAdded += Bot_ReactionAdded;
                        await theMsg.AddReactionAsync("🔍");
                        return;
                    }
                }
            }
            if (Program.companionArray != null) {
                //Third search companions
                for (int i = 0; i < Program.companionArray.Length; i++) {
                    if (itemName == Program.companionArray[i].name + "]") {
                        EmbedBuilder itemBuilder = new EmbedBuilder() {
                            Author = new EmbedAuthorBuilder() {
                                Name = Program.companionArray[i].name
                            },
                            ThumbnailUrl = Program.companionArray[i].iconURL,
                            Description = Program.companionArray[i].description,
                            Color = new Color(250, 246, 122),
                        }.AddField(x => { x.WithName("Armour"); x.WithValue(Program.companionArray[i].maxArmour.ToString()); })
                        .AddField(x => { x.WithName("Health"); x.WithValue(Program.companionArray[i].maxHealth.ToString()); });
                        IMessage sentMsg = await Context.Channel.SendMessageAsync("", false, itemBuilder);

                        //Setup reaction listener
                        displayID = sentMsg.Id;
                        lastImage = Program.companionArray[i].iconURL;
                        var theMsg = (RestUserMessage)await sentMsg.Channel.GetMessageAsync(sentMsg.Id);
                        Program.bot.ReactionAdded += Bot_ReactionAdded;
                        await theMsg.AddReactionAsync("🔍");
                        return;
                    }
                }
            }
            if (Program.modArray != null) {
                //Fourth search mods
                for (int i = 0; i < Program.modArray.Length; i++) {
                    if (itemName == Program.modArray[i].name + "]") {
                        System.Drawing.Color convertedColor = System.Drawing.ColorTranslator.FromHtml(GetEnumDescription(Program.modArray[i].rarity));
                        EmbedBuilder itemBuilder = new EmbedBuilder() {
                            Author = new EmbedAuthorBuilder() {
                                Name = Program.modArray[i].name
                            },
                            ThumbnailUrl = Program.modArray[i].iconURL,
                            Description = Program.modArray[i].description,
                            Color = new Color(convertedColor.R, convertedColor.G, convertedColor.B),
                        }.AddField(x => { x.WithName("Drain"); x.WithValue(Program.modArray[i].maxDrain.ToString()); })
                        .AddField(x => { x.WithName("Fusion Level"); x.WithValue(Program.modArray[i].maxRanks.ToString()); });
                        IMessage sentMsg = await Context.Channel.SendMessageAsync("", false, itemBuilder);

                        //Setup reaction listener
                        displayID = sentMsg.Id;
                        lastImage = Program.modArray[i].iconURL;
                        var theMsg = (RestUserMessage)await sentMsg.Channel.GetMessageAsync(sentMsg.Id);
                        Program.bot.ReactionAdded += Bot_ReactionAdded;
                        await theMsg.AddReactionAsync("🔍");
                        return;
                    }
                }
            }
            if (itemName == "John Prodman]") {
                await Context.Channel.SendMessageAsync("The man, the myth, the legend.");
            }
            else if (itemName == "Clem]") {
                await Context.Channel.SendMessageAsync("He's a special guy, you know. Little weird but a hell of a fighter.");
            }
            else if (itemName != "]") {
                await Context.Channel.SendMessageAsync("Sorry, that item cannot be linked to.");
            }

        }

        private async Task Bot_ReactionAdded(Cacheable<IUserMessage, ulong> message, Discord.WebSocket.ISocketMessageChannel channel, Discord.WebSocket.SocketReaction reaction) {
            if (message.Id == displayID && reaction.Emoji.Name == "🔍" && reaction.UserId != 276656859270873088) {
                //This is our message
                await Context.Channel.SendMessageAsync("Imgur: " + lastImage);
            }

        }

        public static string GetEnumDescription(Enum value) {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }

    public class cmdContrib : InteractiveModuleBase {
        [Command("contrib]", RunMode = RunMode.Async), Summary("Contribute to Link Prime's database!")]
        public async Task Contribute(string subCommand, string sCmdArg1 = null, string sCmdArg2 = null, string sCmdArg3 = null) {
            System.Globalization.TextInfo nameConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            //Wizard Academy trains magicians in the art of warframe item creation
            WizardAcademy wizardAcademy = new WizardAcademy();
            switch (subCommand) {
                case "new":
                case "add":
                    if (sCmdArg1 == null || sCmdArg2 == null) {
                        await Context.Channel.SendMessageAsync("Insufficient parameters. Syntax: \"[contrib] add <type> <name>\"");
                        return;
                    }
                    await wizardAcademy.CreateNewEntry(sCmdArg1, nameConverter.ToTitleCase(sCmdArg2), sCmdArg3);
                    break;
                case "edit":
                    if (sCmdArg1 == null || sCmdArg1 == "") {
                        await Context.Channel.SendMessageAsync("Editing existing item, please specify a name");
                        IUserMessage response = await WaitForMessage(Context.Message.Author, Context.Channel);
                        sCmdArg1 = response.Content;
                    }
                    await wizardAcademy.EditEntry(nameConverter.ToTitleCase(sCmdArg1), sCmdArg2, sCmdArg3);
                    break;
                case "delete":
                    if (Context.Message.Author.Id == 232475108503977995) {
                        await Context.Channel.SendMessageAsync("Deleting existing item, please specify a name.");
                        IUserMessage response = await WaitForMessage(Context.Message.Author, Context.Channel);
                        wizardAcademy.DeleteEntry(response.Content);
                    }
                    break;
                case "help":
                    string contribHelpPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ContribHelp.txt");
                    EmbedBuilder contribBuilder = new EmbedBuilder() {
                        Author = new EmbedAuthorBuilder() {
                            Name = "Link Prime Contributor Help",
                        },
                        ThumbnailUrl = "http://i.imgur.com/MG0pc7Q.png",
                        Description = File.ReadAllText(contribHelpPath),
                        Color = new Color(250, 246, 122),
                    }.AddField(x => { x.WithName("List of somethings"); x.WithValue("yo its mi"); });
                    IDMChannel dmChannel = await Context.User.CreateDMChannelAsync();
                    await ReplyAsync("Inbox message awaits the Operator. Check your DM for help.");
                    await dmChannel.SendMessageAsync("", false, contribBuilder);
                    break;
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
                    Name = "Link Prime " + Program.versionNumber + " Help",
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
            await Context.Channel.SendMessageAsync("", false, helpBuilder);
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
            }.AddField(x=> { x.WithName("Version"); x.WithValue(Program.versionNumber); });
            await Context.Channel.SendMessageAsync("", false, infoBuilder);
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
            await dmChannel.SendMessageAsync("\u200BThe Void Relic has been cracked open. Add Link Prime via\nhttps://discordapp.com/oauth2/authorize?client_id=276370292052459523&scope=bot&permissions=52224");
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
                    Name = "Link Prime " + Program.versionNumber + " Stats",
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
