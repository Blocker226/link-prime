using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Discord_Link_Prime {
    class Program {

        public DiscordSocketClient bot { get; set; }
        CommandService commands;
        DependencyMap map;

        public static string versionNumber = "A1.0.71";

        public static string[] warframeArray;
        public static string[] commandNames;
        public static string[] commandSummaries;
        public static BotStats loadedStats;
        public static int currentUptime;
        static string statsPath;

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        //Main shit down here
        public async Task Start () {
            bot = new DiscordSocketClient(new DiscordSocketConfig() {
                LogLevel = LogSeverity.Info
            });
            commands = new CommandService();
            map = new DependencyMap();

            //Appdata Roaming Folder 
            string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(dataFolder, "Discord Link Prime");
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);

            //Stats json
#if DEBUG
            statsPath = Path.Combine(specificFolder, @"Teststats.json");
#else
            statsPath = Path.Combine(specificFolder, @"stats.json");
#endif
            if (!File.Exists(statsPath)) {
                LogLine("Stats file not found, creating...", "WARNING   ");
                BotStats botStats = new BotStats();
                botStats.serversJoined = 0;
                botStats.totalUptime = 0;
                string initialStats = JsonConvert.SerializeObject(botStats);
                File.WriteAllText(statsPath, initialStats);
                loadedStats = JsonConvert.DeserializeObject<BotStats>(File.ReadAllText(statsPath));
            }
            else {
                LogLine("Stats file found! Loading...");
                loadedStats = JsonConvert.DeserializeObject<BotStats>(File.ReadAllText(statsPath));
                LogLine("Servers Joined: " + loadedStats.serversJoined + "\nTotal uptime in minutes: " + loadedStats.totalUptime);
            }

            await InstallCommands();
            AttachListeners();

            //Warframe database setup
            List<string> wepList = new List<string>();
            string listPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Namelist.txt");
            var lines = File.ReadAllLines(listPath);
            foreach (var line in lines) {
                if (line.StartsWith("//") || string.IsNullOrWhiteSpace(line)) {
                    continue;
                }
                else {
                    wepList.Add(line.ToUpper());
                }
            }
            warframeArray = wepList.ToArray();

            //Setup logging
            bot.Log += (message) =>
            {
                Console.WriteLine($"{message.ToString()}");
                return Task.CompletedTask;
            };

            string tokenPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Token.txt");
            string token;
#if DEBUG
            token = File.ReadLines(tokenPath).Skip(3).Take(1).First();
            await bot.LoginAsync(TokenType.Bot, token);
            await bot.ConnectAsync();
            LogLine("We are getting closer. Accept this small token as appreciation for your efforts. Add Link Vandal via\nhttps://discordapp.com/oauth2/authorize?client_id=276656859270873088&scope=bot&permissions=52224");
#else
            token = File.ReadLines(tokenPath).Skip(1).Take(1).First();
            await bot.LoginAsync(TokenType.Bot, token);
            await bot.ConnectAsync();
            LogLine("The Void Relic has been cracked open. Add Link Prime via\nhttps://discordapp.com/oauth2/authorize?client_id=276370292052459523&scope=bot&permissions=52224");
#endif
            StatTimer.TimeStats();
            await bot.SetGameAsync("[help] for help");

            await Task.Delay(-1);
        }

        public async Task InstallCommands() {
            // Hook the MessageReceived Event into our Command Handler
            bot.MessageReceived += HandleCommand;
            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            //Add command infomation
            List<string> cmdList = new List<string>();
            List<string> sumList = new List<string>();
            foreach (CommandInfo cmdInfo in commands.Commands) {
                cmdList.Add(cmdInfo.Name);
                sumList.Add(cmdInfo.Summary);
            }
            commandNames = cmdList.ToArray();
            commandSummaries = sumList.ToArray();
        }

        public async Task HandleCommand(SocketMessage messageParam) {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            IResult result;
            // Create a Command Context
            var context = new CommandContext(bot, message);
            // Determine if the message is a command, based on if it has a '[' and ']' or a mention prefix
            if (message.HasCharPrefix('[', ref argPos) || message.HasMentionPrefix(bot.CurrentUser, ref argPos)) {
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed succesfully)
                result = await commands.ExecuteAsync(context, argPos, map);
            }
            else if((message.Content.Contains("wf[") && message.Content.Contains("]")) && !(message.Content.Contains("```wf[") && message.Content.Contains("]```"))) {
                var commandRegex = new Regex(@"\[(.*?)\]");
                var commandMatches = commandRegex.Matches(message.Content);
                string[] commandInput = commandMatches.OfType<Match>().Select(m => m.Groups[1].Value).ToArray();
                //LogLine(commandInput[0]);
                result = await commands.ExecuteAsync(context, "link " + commandInput[0] + "]");
            }
            else {
                return;
            }
            if (!result.IsSuccess) {
#if DEBUG
                await context.Channel.SendMessageAsync(result.ErrorReason);
#endif
            }
            
        }

        //Listeners
        private void AttachListeners() {
            bot.JoinedGuild += JoinedGuildHandler;
            bot.LeftGuild += LeftGuildHandler;
        }

        public async Task JoinedGuildHandler(SocketGuild guild) {
            loadedStats.serversJoined++;
            WriteStats();
            IDMChannel dmChannel = await bot.GetGuild(276372931259531265).GetUser(232475108503977995).CreateDMChannelAsync();
            await dmChannel.SendMessageAsync("Link Prime has been added to " + guild.Name + "! Rejoice!");
        }

        public async Task LeftGuildHandler(SocketGuild guild) {
            if (loadedStats.serversJoined > 0) {
                loadedStats.serversJoined--;
            }
            WriteStats();
            IDMChannel dmChannel = await bot.GetGuild(276372931259531265).GetUser(232475108503977995).CreateDMChannelAsync();
            await dmChannel.SendMessageAsync("Link Prime has been removed from " + guild.Name + "! Noooo!");
        }

        public static void WriteStats() {
            string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(dataFolder, "Discord Link Prime");
            string newStats = JsonConvert.SerializeObject(loadedStats);
            File.WriteAllText(statsPath, newStats);
#if DEBUG
            LogLine("Stats updated!");
#endif
        }

        public static void LogLine(string msg, string tag = "Info      ") {
            //tag must be 7 characters + 3 spaces long
            msg = msg.Replace("\n", "\n                   ");
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + tag + msg);
        }
    }
}
