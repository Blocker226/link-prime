using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Discord_Link_Prime {
    class Program {

        DiscordSocketClient bot;
        CommandService commands;
        DependencyMap map;

        public static string[] warframeArray;

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        //Main shit down here
        public async Task Start () {
            bot = new DiscordSocketClient(new DiscordSocketConfig() {
                LogLevel = LogSeverity.Info
            });
            commands = new CommandService();
            map = new DependencyMap();

            await InstallCommands();

            //Warframe database setup
            List<string> wepList = new List<string>();
            string listPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Namelist.txt");
            var lines = File.ReadAllLines(listPath);
            foreach (var line in lines) {
                if (line.StartsWith("//") || string.IsNullOrWhiteSpace(line)) {
                    continue;
                }
                else {
                    wepList.Add(line);
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
            Console.WriteLine("We are getting closer. Accept this small token as appreciation for your efforts. Add Link Vandal via\nhttps://discordapp.com/oauth2/authorize?client_id=276656859270873088&scope=bot&permissions=35840");
#else
            token = File.ReadLines(tokenPath).Skip(1).Take(1).First();
            await bot.LoginAsync(TokenType.Bot, token);
            await bot.ConnectAsync();
            Console.WriteLine("The Void Relic has been cracked open. Add Link Prime via\nhttps://discordapp.com/oauth2/authorize?client_id=276370292052459523&scope=bot&permissions=35840");

#endif
            await bot.SetGameAsync("[help] for help");

            await Task.Delay(-1);
        }

        public async Task InstallCommands() {
            // Hook the MessageReceived Event into our Command Handler
            bot.MessageReceived += HandleCommand;
            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam) {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it has a '[' and ']' or a mention prefix
            if (!(message.HasCharPrefix('[', ref argPos) || message.HasMentionPrefix(bot.CurrentUser, ref argPos))) return;
            //if (!((message.Content.Contains("[") && message.Content.Contains("]")) || message.HasMentionPrefix(bot.CurrentUser, ref argPos))) return;
            var commandRegex = new Regex(@"\[(.*?)\]");
            var commandMatches = commandRegex.Matches(message.Content);
            string[] commandInput = commandMatches.OfType<Match>().Select(m => m.Groups[1].Value).ToArray();
            Console.WriteLine(commandInput[0]);
            // Create a Command Context
            var context = new CommandContext(bot, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed succesfully)
            var result = await commands.ExecuteAsync(context, argPos, map);
            //var result = await commands.ExecuteAsync(context, commandInput[0] + "]");
            if (!result.IsSuccess) {
#if DEBUG
                await context.Channel.SendMessageAsync(result.ErrorReason);
#endif
            }
        }
    }
}
