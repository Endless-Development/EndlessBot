// THIS CODE IS PART OF ENDLESS NETWORK BOT (made by Kheeto)
// License:
/* 
 * Copyright (c) 2021 Kheeto

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR 
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace EndlessNetworkBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }

        /// <summary>
        /// Start and setup the bot.
        /// </summary>
        public async Task StartAsync()
        {
            // loads main config and starts bot
            DiscordConfiguration discord = GetDiscordConfiguration();
            Client = new DiscordClient(discord);
            Client.Ready += OnReady; // gets called when the bot is ready
            
            // loads commandsnext
            CommandsNextConfiguration commandsNext = GetCommandsNextConfiguration();
            Commands = Client.UseCommandsNext(commandsNext);
            Commands.SetHelpFormatter<HelpFormatter>();
            await RegisterCommandsAsync().ConfigureAwait(false);

            // loads interactivity
            InteractivityConfiguration interactivity = GetInteractivityConfiguration();
            Client.UseInteractivity(interactivity);

            // loads voicenext
            VoiceNextConfiguration voiceNext = GetVoiceNextConfiguration();
            Client.UseVoiceNext(voiceNext);

            // finally connects the bot to discord
            await Client.ConnectAsync(null, UserStatus.Invisible).ConfigureAwait(false);

            await Task.Delay(-1);
        }

        #region Registering Commands

        /// <summary>
        /// Register the bot commands
        /// </summary>
        /// <returns>Task.CompletedTask</returns>
        private async Task<Task> RegisterCommandsAsync()
        {
            // registering commands here

            return Task.CompletedTask;
        }

        #endregion

        #region Events

        /// <summary>
        /// Gets called when the bot is ready
        /// </summary>
        private async Task OnReady(DiscordClient client, ReadyEventArgs ev)
        {
            // sets custom activity
            DiscordActivity activity = new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = GetConfig().Result.Bot_Status,
            };

            // updates bot activity
            await Client.UpdateStatusAsync(activity, UserStatus.DoNotDisturb);

            Console.WriteLine(GetConfig().Result.Message_onReady);
        }

        #endregion

        #region Loading Configurations

        /// <returns>Returns the Discord Configuration</returns>
        private DiscordConfiguration GetDiscordConfiguration()
        {
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = GetConfig().Result.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                AlwaysCacheMembers = true,
                Proxy = null,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                MessageCacheSize = 2048,
                UseRelativeRatelimit = false,
            };

            return config;
        }

        /// <returns>Returns the CommandsNext Configuration</returns>
        private CommandsNextConfiguration GetCommandsNextConfiguration()
        {
            CommandsNextConfiguration config = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { GetConfig().Result.Prefix },
                EnableDms = false,
                EnableDefaultHelp = true,
                EnableMentionPrefix = true,
                CaseSensitive = false,
            };

            return config;
        }

        /// <returns>Returns the Interactivity Configuration</returns>
        private InteractivityConfiguration GetInteractivityConfiguration()
        {
            PaginationEmojis paginationEmojis = new PaginationEmojis();
            paginationEmojis.Left = DiscordEmoji.FromName(Client, ":arrow_left:");
            paginationEmojis.Right = DiscordEmoji.FromName(Client, ":arrow_right:");
            paginationEmojis.SkipLeft = DiscordEmoji.FromName(Client, ":rewind:");
            paginationEmojis.SkipRight = DiscordEmoji.FromName(Client, ":fast_forward:");
            paginationEmojis.Stop = DiscordEmoji.FromName(Client, ":octagonal_sign:");

            InteractivityConfiguration config = new InteractivityConfiguration
            {
                PaginationBehaviour = DSharpPlus.Interactivity.Enums.PaginationBehaviour.WrapAround,
                PaginationDeletion = DSharpPlus.Interactivity.Enums.PaginationDeletion.DeleteEmojis,
                PaginationEmojis = paginationEmojis,
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.DeleteEmojis,
                Timeout = new TimeSpan(0, 0, 60),
            };

            return config;
        }

        /// <returns>Returns the VoiceNext Configuration</returns>
        private VoiceNextConfiguration GetVoiceNextConfiguration()
        {
            VoiceNextConfiguration config = new VoiceNextConfiguration
            {
                AudioFormat = AudioFormat.Default,
                EnableIncoming = false,
                PacketQueueSize = 25,
            };

            return config;
        }

        #endregion

        #region Loading JSON Config

       public async Task<JSONConfig> GetConfig()
       {
            string config = null;

            FileStream fs = File.OpenRead("config.json");
            StreamReader sr = new StreamReader(fs, new UTF8Encoding(false));
            config = await sr.ReadToEndAsync().ConfigureAwait(false);

            JSONConfig json = JsonConvert.DeserializeObject<JSONConfig>(config);

            return json;
       }

        #endregion
    }

    public struct JSONConfig
    {
        [JsonProperty("token")] public string Token { get; private set; }
        [JsonProperty("prefix")] public string Prefix { get; private set; }
        [JsonProperty("bot_status")] public string Bot_Status { get; private set; }

        #region Help Properties

        [JsonProperty("help_Color")] public string Help_Color { get; private set; }
        [JsonProperty("help_Title")] public string Help_Title { get; private set; }
        [JsonProperty("help_Description")] public string Help_Description { get; private set; }
        [JsonProperty("help_WithCommand_command")] public string Help_WithCommand_command { get; private set; }
        [JsonProperty("help_WithCommand_arguments")] public string Help_WithCommand_arguments { get; private set; }
        [JsonProperty("help_WithCommand_stringBuilder")] public string Help_WithCommand_stringBuilder { get; private set; }
        [JsonProperty("help_WithCommand_noArguments")] public string Help_WithCommand_noArguments { get; private set; }
        [JsonProperty("help_WithSubCommands_Description")] public string Help_WithSubCommands_Description { get; private set; }
        [JsonProperty("help_WithSubCommands_Commands")] public string Help_WithSubCommands_Commands { get; private set; }

        #endregion

        #region Messages

        [JsonProperty("message_onReady")] public string Message_onReady { get; private set; }

        #endregion
    }

}
